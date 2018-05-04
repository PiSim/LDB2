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
using System.Windows;
using System.Collections;
using System.Windows.Forms;

namespace Reports.ViewModels
{   
    public class ReportCreationDialogViewModel : BindableBase, INotifyDataErrorInfo, IRequirementSelector 
    {
        public enum CreationModes
        {
            Report,
            ReportFromTask,
            Task
        }
        
        private bool _isCreatingFromTask;
        private Batch _selectedBatch;
        private ControlPlan _selectedControlPlan;
        private CreationModes _creationMode;
        private DBPrincipal _principal;
        private DelegateCommand<Window> _cancel, _confirm;
        private readonly Dictionary<string, ICollection<string>> _validationErrors = new Dictionary<string, ICollection<string>>();
        private EventAggregator _eventAggregator;
        private IDataService _dataService;
        private IEnumerable<ControlPlan> _controlPlanList;
        private IEnumerable<ISelectableRequirement> _requirementList;
        private IEnumerable<Person> _techList;
        private IEnumerable<Specification> _specificationList;
        private IEnumerable<SpecificationVersion> _versionList;
        private IEnumerable<ITestItem> _taskItemList;
        private IReportService _reportService;
        private Int32 _number;
        private Material _material;
        private Person _author;
        private Report _reportInstance;
        private Specification _selectedSpecification;
        private SpecificationVersion _selectedVersion;
        private string _batchNumber, 
                        _category,
                        _description;
        private Task _taskInstance;
        
        public ReportCreationDialogViewModel(DBPrincipal principal,
                                            EventAggregator aggregator,
                                            IDataService dataService,
                                            IReportService reportService) : base()
        {
            _dataService = dataService;
            _eventAggregator = aggregator;
            _reportService = reportService;
            _principal = principal;
            _creationMode = CreationModes.Report;
            _isCreatingFromTask = false;

            _number = _reportService.GetNextReportNumber();
            _techList = _dataService.GetPeople(PersonRoleNames.MaterialTestingTech);
            _author = _techList.First(prs => prs.ID == _principal.CurrentPerson.ID);
            _requirementList = new List<ISelectableRequirement>();
            _specificationList = _dataService.GetSpecifications();

            _confirm = new DelegateCommand<Window>(
                parent => {

                    // Checks if DoNotTest flag is enabled and asks for user confirmation if it is

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
            _confirm.RaiseCanExecuteChanged();
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

                SelectedBatch = _dataService.GetBatch(_batchNumber);
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
                return _controlPlanList;
            }
        }
        
        public bool ControlPlanSelectionEnabled => !IsCreatingFromTask;

        public CreationModes CreationMode
        {
            get => _creationMode;
            set
            {
                _creationMode = value;

                IsCreatingFromTask = (_creationMode == CreationModes.Report);
                
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

        public bool IsCreatingFromTask
        {
            get => _isCreatingFromTask;
            private set
            {
                _isCreatingFromTask = value;
                RaisePropertyChanged();
                RaisePropertyChanged("ControlPlanSelectionEnabled");
                RaisePropertyChanged("TestSelectionColumnVisibility");
                RaisePropertyChanged("SpecificationSelectionEnabled");
                RaisePropertyChanged("VersionSelectionEnabled");
            }
        }

        public bool IsNotAdmin => !_principal.IsInRole(UserRoleNames.ReportAdmin);

        public bool IsSpecificationSelected => _selectedSpecification != null;

        public Material Material => _material;

        public Int32 Number
        {
            get { return _number; }
            set
            {
                _number = value;
                Report tempReport = _dataService.GetReportByNumber(_number);

                if (tempReport != null)
                    _validationErrors["Number"] = new List<string>() { "Il report " + value + " esiste già" };

                else if (_validationErrors.ContainsKey("Number"))
                    _validationErrors.Remove("Number");

                _confirm.RaiseCanExecuteChanged();
                RaiseErrorsChanged("Number");
            }
        }

        public IEnumerable<ITestItem> RequirementList
        {
            get
            {
                if (!IsCreatingFromTask)
                    return _requirementList;

                else
                    return _taskItemList;
            }
        }

        public Report ReportInstance => _reportInstance; 

        public Batch SelectedBatch
        {
            get { return _selectedBatch; }
            set
            {
                _selectedBatch = value;

                // Updates Error Status of the Property

                if (_selectedBatch == null)
                    _validationErrors["BatchNumber"] = new List<string>() { "Batch inserito non valido" };

                else if (_validationErrors.ContainsKey("BatchNumber"))
                    _validationErrors.Remove("BatchNumber");
                
                // Attempts to Retrieve the BAtch Material

                _material = _selectedBatch.GetMaterial();
                RaisePropertyChanged("ExternalConstruction");
                RaisePropertyChanged("Material");

                // If Using a Task as Template no further action is required

                if (IsCreatingFromTask)
                    return;

                // Otherwise sets the default selected values if the material and the external construction are not null

                if (_material != null && _material.ExternalConstruction != null)
                {
                    SpecificationVersion tempVersion = _dataService.GetSpecificationVersion((int)_material.ExternalConstruction.DefaultSpecVersionID);
                    SelectedSpecification = SpecificationList.FirstOrDefault(spec => spec.ID == tempVersion.SpecificationID);
                    SelectedVersion = VersionList.First(vers => vers.ID == _material.ExternalConstruction.DefaultSpecVersionID);
                }
            }
        }

        public ControlPlan SelectedControlPlan
        {
            get { return _selectedControlPlan; }
            set
            {
                _selectedControlPlan = value;
                RaisePropertyChanged("SelectedControlPlan");

                // If new value is not null and a Task is not being used as template
                // sets Report description and applies the control Plan
                
                if (_selectedControlPlan != null)
                {
                    Description = (_selectedControlPlan == null) ? "" : _selectedControlPlan.Name;
                    _reportService.ApplyControlPlan(_requirementList, _selectedControlPlan);
                }
            }
        }

        public IEnumerable<Requirement> SelectedRequirements => _requirementList.Where(req => req.IsSelected)
                                                                                .Select(req => req.RequirementInstance);

        public Specification SelectedSpecification
        {
            get { return _selectedSpecification; }
            set
            {
                _selectedSpecification = value;

                // Raises change of dependent properties

                RaisePropertyChanged("ControlPlanSelectionEnabled");
                RaisePropertyChanged("VersionSelectionEnabled");
                RaisePropertyChanged("SelectedSpecification");

                // Updates Error Status of the Property

                if (_selectedSpecification == null)
                    _validationErrors["SelectedSpecification"] = new List<string>() { "Selezionare una specifica" };
                
                else if (_validationErrors.ContainsKey("SelectedSpecification"))
                        _validationErrors.Remove("SelectedSpecification");

                RaiseErrorsChanged("SelectedSpecification");

                // If instance is null sets empty children lists and null selected values

                if (_selectedSpecification == null)
                {
                    _controlPlanList = new List<ControlPlan>();
                    RaisePropertyChanged("ControlPlanList");

                    _versionList = new List<SpecificationVersion>();
                    RaisePropertyChanged("VersionList");

                    SelectedControlPlan = null;
                    SelectedVersion = null;
                }

                // Otherwise retrieves children list and loads default selected values

                else
                {
                    _controlPlanList = _selectedSpecification.GetControlPlans();
                    RaisePropertyChanged("ControlPlanList");

                    _versionList = _selectedSpecification?.GetVersions();
                    RaisePropertyChanged("VersionList");

                    SelectedControlPlan = ControlPlanList.FirstOrDefault(cp => cp.IsDefault);
                    SelectedVersion = VersionList.FirstOrDefault(sv => sv.IsMain);
                }
            }
        }

        public SpecificationVersion SelectedVersion
        {
            get { return _selectedVersion; }
            set 
            { 
                _selectedVersion = value;
                RaisePropertyChanged("SelectedVersion");
                
                // If Using Task as template no further action is required

                if (IsCreatingFromTask)
                    return;

                // If new value is null sets empty requirementList

                if (_selectedVersion == null)
                    _requirementList = new List<ISelectableRequirement>();

                // Otherwise Retrieves new requirementList 

                else
                {
                    _requirementList = _selectedVersion.GenerateRequirementList()
                                                        .Select(req => new ReportItemWrapper(req, this))
                                                        .ToList();
                    _reportService.ApplyControlPlan(_requirementList, _selectedControlPlan);
                }

                RaisePropertyChanged("RequirementList");
            }
        }

        public IEnumerable<Specification> SpecificationList => _specificationList;

        public bool SpecificationSelectionEnabled => !IsCreatingFromTask;

        public IEnumerable<SpecificationVersion> VersionList => _versionList;

        public bool VersionSelectionEnabled => (SelectedSpecification != null) && !IsCreatingFromTask;

        public IEnumerable<Person> TechList => _techList;

        /// <summary>
        /// Allows using a Task entity as basis for creating the report.
        /// </summary>
        public Task TaskInstance
        {
            get => _taskInstance;
            set
            {
                // Sets value
                _taskInstance = value;

                // Sets the Batch instance
                BatchNumber = _taskInstance.Batch.Number;

                // Selects correct specification 
                SelectedSpecification = _specificationList.First(spc => spc.ID == _taskInstance.SpecificationVersion.SpecificationID);
                RaisePropertyChanged("SelectedSpecification");

                // Selects correct Version
                _selectedVersion = _versionList.First(spv => spv.ID == _taskInstance.SpecificationVersionID);
                RaisePropertyChanged("SelectedVersion");

                // Gets Test List from the task
                _taskItemList = _taskInstance.GetTaskItems()
                                            .Select(tski => new TaskItemWrapper(tski));
                RaisePropertyChanged("RequirementList");
            }
        }

        public Visibility TestSelectionColumnVisibility => IsCreatingFromTask ? Visibility.Collapsed : Visibility.Visible;

        public double? TotalDuration => _requirementList.Where(req => req.IsSelected)
                                                        .Sum(req => req.WorkHours);        

    }
}