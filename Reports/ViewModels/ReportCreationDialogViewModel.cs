using DataAccess;
using Infrastructure;
using Infrastructure.Commands;
using Infrastructure.Queries;
using Infrastructure.Wrappers;
using LabDbContext;
using LabDbContext.EntityExtensions;
using LabDbContext.Services;
using LInst;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Reports.Queries;
using Specifications.Queries;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows;

namespace Reports.ViewModels
{
    public class ReportCreationDialogViewModel : BindableBase, INotifyDataErrorInfo, IRequirementSelector
    {
        #region Fields

        private readonly Dictionary<string, ICollection<string>> _validationErrors = new Dictionary<string, ICollection<string>>();

        private LInst.Person _author;

        private string _batchNumber, _description;

        private IEventAggregator _eventAggregator;
        private IDataService<LabDbEntities> _labDbData;
        DataAccessCore.IDataService<LInstContext> _lInstData;
        private Int32 _number;

        private IReportService _reportService;

        private IEnumerable<ISelectableRequirement> _requirementList;

        private Batch _selectedBatch;

        private ControlPlan _selectedControlPlan;

        private Specification _selectedSpecification;

        private SpecificationVersion _selectedVersion;

        #endregion Fields

        #region Constructors

        public ReportCreationDialogViewModel(IDataService<LabDbEntities> labDbData,
                                            IEventAggregator aggregator,
                                            IReportService reportService, DataAccessCore.IDataService<LInstContext> lInstData) : base()
        {
            _labDbData = labDbData;
            _eventAggregator = aggregator;
            _reportService = reportService;
            _lInstData = lInstData;

            _number = _reportService.GetNextReportNumber();
            TechList = _lInstData.RunQuery(new PeopleQuery() { Role = PeopleQuery.PersonRoles.MaterialTestingTech })
                                                            .ToList();
            _author = TechList.First(prs => prs.ID == (Thread.CurrentPrincipal as DBPrincipal).CurrentPerson.ID);
            _requirementList = new List<ISelectableRequirement>();
            SpecificationList = _labDbData.RunQuery(new SpecificationsQuery()).ToList();

            ConfirmCommand = new DelegateCommand<Window>(
                parent =>
                {
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
                        _labDbData.Execute(new UpdateEntityCommand(_selectedBatch));
                    }
                    parent.DialogResult = true;
                },
                parent => !HasErrors);

            CancelCommand = new DelegateCommand<Window>(
                parent =>
                {
                    parent.DialogResult = false;
                });

            SelectedSpecification = null;
            BatchNumber = "";
        }

        #endregion Constructors

        #region Properties

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

                SelectedBatch = _labDbData.RunQuery(new BatchQuery() { Number = _batchNumber });
                RaisePropertyChanged("BatchNumber");

                RaiseErrorsChanged("BatchNumber");
            }
        }

        public DelegateCommand<Window> CancelCommand { get; }

        public string Category { get; set; }

        public DelegateCommand<Window> ConfirmCommand { get; }

        public IEnumerable<ControlPlan> ControlPlanList { get; private set; }
        
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
                if (Material == null || Material.ExternalConstruction == null)
                    return null;

                return Material.ExternalConstruction.Name;
            }
        }

        public bool IsSpecificationSelected => _selectedSpecification != null;

        public Material Material { get; private set; }

        public Int32 Number
        {
            get { return _number; }
            set
            {
                _number = value;
                Report tempReport = _labDbData.RunQuery(new ReportQuery() { Number = _number });

                if (tempReport != null)
                    _validationErrors["Number"] = new List<string>() { "Il report " + value + " esiste già" };
                else if (_validationErrors.ContainsKey("Number"))
                    _validationErrors.Remove("Number");

                ConfirmCommand.RaiseCanExecuteChanged();
                RaiseErrorsChanged("Number");
            }
        }

        public IEnumerable<ITestItem> RequirementList => _requirementList;

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

                Material = _selectedBatch?.Material;
                RaisePropertyChanged("ExternalConstruction");
                RaisePropertyChanged("Material");

                // Otherwise sets the default selected values if the material and the external construction are not null

                if (Material != null && Material.ExternalConstruction != null)
                {
                    SpecificationVersion tempVersion = _labDbData.RunQuery(new SpecificationVersionQuery() { ID = Material.ExternalConstruction.DefaultSpecVersionID });
                    SelectedSpecification = SpecificationList.FirstOrDefault(spec => spec.ID == tempVersion.SpecificationID);
                    SelectedVersion = VersionList.First(vers => vers.ID == Material.ExternalConstruction.DefaultSpecVersionID);
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
                    ControlPlanList = new List<ControlPlan>();
                    RaisePropertyChanged("ControlPlanList");

                    VersionList = new List<SpecificationVersion>();
                    RaisePropertyChanged("VersionList");

                    SelectedControlPlan = null;
                    SelectedVersion = null;
                }

                // Otherwise retrieves children list and loads default selected values
                else
                {
                    ControlPlanList = (_selectedSpecification == null) ? null : _labDbData.RunQuery(new ControlPlansQuery() { SpecificationID = _selectedSpecification.ID }).ToList();
                    RaisePropertyChanged("ControlPlanList");

                    VersionList = (_selectedSpecification == null) ? null : _labDbData.RunQuery(new SpecificationVersionsQuery() { SpecificationID = _selectedSpecification.ID }).ToList();
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

        public IEnumerable<Specification> SpecificationList { get; }
                
        public IEnumerable<Person> TechList { get; }

        public double? TotalDuration => _requirementList.Where(req => req.IsSelected)
                                                        .Sum(req => req.WorkHours);

        public IEnumerable<SpecificationVersion> VersionList { get; private set; }

        public bool VersionSelectionEnabled => SelectedSpecification != null;

        #endregion Properties

        #region Methods

        public void OnRequirementSelectionChanged()
        {
            RaisePropertyChanged("TotalDuration");
        }

        #endregion Methods

        #region INotifyDataErrorInfo interface elements

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public bool HasErrors => _validationErrors.Count > 0;

        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName)
                || !_validationErrors.ContainsKey(propertyName))
                return null;

            return _validationErrors[propertyName];
        }

        private void RaiseErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            RaisePropertyChanged("HasErrors");
            ConfirmCommand.RaiseCanExecuteChanged();
        }

        #endregion INotifyDataErrorInfo interface elements
    }
}