using DBManager;
using DBManager.EntityExtensions;
using DBManager.Services;
using Infrastructure;
using Infrastructure.Events;
using Infrastructure.Wrappers;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Collections;
using System.Windows.Forms;

namespace Services.ViewModels
{   
    public class ReportCreationDialogViewModel : BindableBase, INotifyDataErrorInfo, IRequirementSelector 
    {
        private Batch _selectedBatch;
        private ControlPlan _selectedControlPlan;
        private DBPrincipal _principal;
        private DelegateCommand<Window> _cancel, _confirm;
        private readonly Dictionary<string, ICollection<string>> _validationErrors = new Dictionary<string, ICollection<string>>();
        private EventAggregator _eventAggregator;
        private IEnumerable<ISelectableRequirement> _requirementList;
        private IEnumerable<Person> _techList;
        private IEnumerable<Specification> _specificationList;
        private Int32 _number;
        private Material _material;
        private Person _author;
        private Report _reportInstance;
        private Specification _selectedSpecification;
        private SpecificationVersion _selectedVersion;
        private string _batchNumber, 
                        _category,
                        _description;
        
        public ReportCreationDialogViewModel(DBPrincipal principal,
                                            EventAggregator aggregator) : base()
        {
            _eventAggregator = aggregator;
            _principal = principal;

            _number = ReportService.GetNextReportNumber();
            _techList = PeopleService.GetPeople(PersonRoleNames.MaterialTestingTech);
            _author = _techList.First(prs => prs.ID == _principal.CurrentPerson.ID);
            _requirementList = new List<ISelectableRequirement>();
            _specificationList = SpecificationService.GetSpecifications();

            _confirm = new DelegateCommand<Window>(
                parent => {

                    if (_selectedBatch.DoNotTest)
                    {
                        string confirmationMessage = "Nel batch " + _selectedBatch.Number + " è impostato il flag \"Non effettuare test su questo batch\".\nContinuando il flag verrà rimosso, continuare?";
                        MessageBoxResult confirmationResult = System.Windows.MessageBox.Show(confirmationMessage,
                                                                                            "Conferma",
                                                                                            MessageBoxButton.YesNo);

                        if (confirmationResult != MessageBoxResult.Yes)
                            return;

                        _selectedBatch.DoNotTest = false;
                        _selectedBatch.Update();
                    }                                       

                    _reportInstance = new Report();
                    _reportInstance.AuthorID = _author.ID;
                    _reportInstance.BatchID = _selectedBatch.ID;
                    _reportInstance.Category = "TR";
                    if (_selectedControlPlan != null)
                        _reportInstance.Description = _selectedControlPlan.Name;
                    else
                        _reportInstance.Description = _description;
                    _reportInstance.IsComplete = false;
                    _reportInstance.Number = _number;
                    _reportInstance.SpecificationVersionID = _selectedVersion.ID;
                    _reportInstance.StartDate = DateTime.Now.ToShortDateString();

                    foreach (Test tst in CommonProcedures.GenerateTestList(_requirementList))
                        _reportInstance.Tests.Add(tst);

                    _reportInstance.TotalDuration = _reportInstance.Tests.Sum(tst => tst.Duration);

                    _reportInstance.Create();
                    parent.DialogResult = true;
                },
                parent => !HasErrors);
                
            _cancel = new DelegateCommand<Window>(
                parent => {
                    parent.DialogResult = false;    
                });
            
            SelectedSpecification = null;
            BatchNumber = "";
        }

        public void OnRequirementSelectionChanged()
        {
            RaisePropertyChanged("TotalDuration");
        }

        #region INotifyDataErrorInfo interface elements

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName)
                || !_validationErrors.ContainsKey(propertyName))
                return null;

            return _validationErrors[propertyName];
        }

        public bool HasErrors
        {
            get { return _validationErrors.Count > 0; }
        }

        private void RaiseErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            RaisePropertyChanged("HasErrors");
        }

        #endregion

        public Person Author
        {
            get { return _author; }
            set 
            { 
                _author = value; 
                RaisePropertyChanged("Author");
            }
        }

        public string BatchNumber
        {
            get { return _batchNumber; }
            set
            {
                _batchNumber = value;

                SelectedBatch = MaterialService.GetBatch(_batchNumber);
                RaisePropertyChanged("BatchNumber");

                RaiseErrorsChanged("BatchNumber");
            }
        }
        
        public DelegateCommand<Window> CancelCommand
        {
            get { return _cancel; }
        }

        public string Category
        {
            get { return _category; }
            set { _category = value; }
        }
        
        public DelegateCommand<Window> ConfirmCommand
        {
            get { return _confirm; }
        }

        public IEnumerable<ControlPlan> ControlPlanList
        {
            get
            {
                if (_selectedSpecification == null)
                    return new List<ControlPlan>();
                else
                    return _selectedSpecification.ControlPlans;
            }
        }

        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                RaisePropertyChanged("Description");
            }
        }
        
        public string ExternalConstruction
        {
            get
            {
                if (_material == null || _material.ExternalConstruction == null)
                    return null;

                return _material.ExternalConstruction.Name;
            }
        }

        public bool IsSpecificationSelected
        {
            get { return _selectedSpecification != null; }
        }

        public bool IsValidInput
        {
            get { return true; }
        }
        
        public Material Material
        {
            get
            {
                return _material;
            }
        }

        public Int32 Number
        {
            get { return _number; }
            set
            {
                _number = value;
                Report tempReport = ReportService.GetReportByNumber(_number);

                if (tempReport != null)
                    _validationErrors["Number"] = new List<string>() { "Il report " + value + " esiste già" };

                else if (_validationErrors.ContainsKey("Number"))
                    _validationErrors.Remove("Number");

                _confirm.RaiseCanExecuteChanged();
                RaiseErrorsChanged("Number");
            }
        }

        public IEnumerable<ISelectableRequirement> RequirementList
        {
            get { return _requirementList; }
        }

        public Report ReportInstance
        {
            get { return _reportInstance; }
        }

        public Batch SelectedBatch
        {
            get { return _selectedBatch; }
            set
            {
                _selectedBatch = value;

                if (_selectedBatch == null)
                    _validationErrors["BatchNumber"] = new List<string>() { "Batch inserito non valido" };

                else if (_validationErrors.ContainsKey("BatchNumber"))
                    _validationErrors.Remove("BatchNumber");
                
                _material = _selectedBatch.GetMaterial();

                if (_material != null && _material.ExternalConstruction != null)
                {
                    SpecificationVersion tempVersion = SpecificationService.GetSpecificationVersion((int)_material.ExternalConstruction.DefaultSpecVersionID);
                    SelectedSpecification = SpecificationList.FirstOrDefault(spec => spec.ID == tempVersion.SpecificationID);
                    SelectedVersion = VersionList.First(vers => vers.ID == tempVersion.ID);
                }

                RaisePropertyChanged("ExternalConstruction");
                RaisePropertyChanged("Material");
            }
        }

        public ControlPlan SelectedControlPlan
        {
            get { return _selectedControlPlan; }
            set
            {
                _selectedControlPlan = value;
                _selectedControlPlan.Load();

                Description = (_selectedControlPlan == null) ? "" : _selectedControlPlan.Name;

                RaisePropertyChanged("SelectedControlPlan");
                if (value != null)
                {
                    CommonProcedures.ApplyControlPlan(_requirementList, _selectedControlPlan);
                }
            }
        }

        public Specification SelectedSpecification
        {
            get { return _selectedSpecification; }
            set
            {
                _selectedSpecification = value;
                _selectedSpecification.Load();

                if (_selectedSpecification == null)
                    _validationErrors["SelectedSpecification"] = new List<string>() { "Selezionare una specifica" };

                else if (_validationErrors.ContainsKey("SelectedSpecification"))
                    _validationErrors.Remove("SelectedSpecification");

                RaisePropertyChanged("IsSpecificationSelected");
                RaisePropertyChanged("SelectedSpecification");
                RaisePropertyChanged("VersionList");
                RaisePropertyChanged("ControlPlanList");
                RaiseErrorsChanged("SelectedSpecification");

                _confirm.RaiseCanExecuteChanged();

                SelectedControlPlan = ControlPlanList.FirstOrDefault(cp => cp.IsDefault);
                SelectedVersion = VersionList.FirstOrDefault(sv => sv.IsMain);
            }
        }

        public SpecificationVersion SelectedVersion
        {
            get { return _selectedVersion; }
            set 
            { 
                _selectedVersion = value;
                _selectedVersion.Load();

                if (_selectedVersion != null)
                {
                    _requirementList = _selectedVersion.GenerateRequirementList()
                                                        .Select(req => new ReportItemWrapper(req, this))
                                                        .ToList();
                    CommonProcedures.ApplyControlPlan(_requirementList, _selectedControlPlan);
                }

                else
                    _requirementList = new List<ISelectableRequirement>();

                RaisePropertyChanged("RequirementList");
                RaisePropertyChanged("SelectedVersion");
            }
        }
        
        public IEnumerable<Specification> SpecificationList
        {
            get { return _specificationList; }
        }
        
        public IEnumerable<SpecificationVersion> VersionList
        {
            get
            {
                if (_selectedSpecification == null)
                    return new List<SpecificationVersion>();

                else
                    return _selectedSpecification.SpecificationVersions;
            }
        }

        public IEnumerable<Person> TechList
        {
            get
            {
                return _techList;
            }
        }

        public double TotalDuration
        {
            get
            {
                return _requirementList.Where(req => req.IsSelected)
                                        .Sum(req => req.Duration);
            }
        }
    }
}