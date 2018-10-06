using DataAccess;
using Infrastructure;
using Infrastructure.Queries;
using Infrastructure.Wrappers;
using LabDbContext;
using LabDbContext.EntityExtensions;
using LabDbContext.Services;
using Prism.Commands;
using Prism.Mvvm;
using Specifications.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows;

namespace Tasks.ViewModels
{
    public class TaskCreationDialogViewModel : BindableBase, IRequirementSelector
    {
        #region Fields

        private string _batchNumber;
        private IDataService _dataService;
        private IDataService<LabDbEntities> _labDbData;
        private IReportService _reportService;
        private Person _requester;
        private Batch _selectedBatch;
        private ControlPlan _selectedControlPlan;
        private Specification _selectedSpecification;
        private SpecificationVersion _selectedVersion;

        #endregion Fields

        #region Constructors

        public TaskCreationDialogViewModel(IDataService<LabDbEntities> labDbData,
                                            IDataService dataService,
                                            IReportService reportService) : base()
        {
            _labDbData = labDbData;
            _dataService = dataService;
            _reportService = reportService;

            LeaderList = _labDbData.RunQuery(new PeopleQuery() { Role = PeopleQuery.PersonRoles.CalibrationTech })
                                                            .ToList();
            SpecificationList = _labDbData.RunQuery(new SpecificationsQuery()).ToList();
            RequirementList = new List<ReportItemWrapper>();

            _requester = LeaderList.FirstOrDefault(prs => prs.ID == (Thread.CurrentPrincipal as DBPrincipal).CurrentPerson.ID);

            CancelCommand = new DelegateCommand<Window>(
                parent =>
                {
                    parent.DialogResult = false;
                });

            ConfirmCommand = new DelegateCommand<Window>(
                parent =>
                {
                    Batch tempBatch = _labDbData.RunQuery(new BatchQuery() { Number = _batchNumber });
                    TaskInstance = new Task
                    {
                        BatchID = tempBatch.ID,
                        Notes = Notes,
                        PipelineOrder = 0,
                        PriorityModifier = 0,
                        Progress = 0,
                        RequesterID = _requester.ID,
                        SpecificationVersionID = _selectedVersion.ID,
                        StartDate = DateTime.Now.Date,
                        WorkHours = TotalDuration
                    };

                    foreach (TaskItem tItem in _reportService.GenerateTaskItemList(RequirementList
                                                                .Where(req => req.IsSelected)
                                                                .Select(req => req.RequirementInstance)))
                        TaskInstance.TaskItems.Add(tItem);

                    TaskInstance.Create();

                    parent.DialogResult = true;
                },
                parent => IsValidInput
            );
        }

        #endregion Constructors

        #region Properties

        public string BatchNumber
        {
            get { return _batchNumber; }
            set
            {
                _batchNumber = value;
                SelectedBatch = _labDbData.RunQuery(new BatchQuery() { Number = _batchNumber });
            }
        }

        public DelegateCommand<Window> CancelCommand { get; }

        public DelegateCommand<Window> ConfirmCommand { get; }

        public IList<ControlPlan> ControlPlanList { get; private set; }

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

        public bool IsValidInput => true;

        public IEnumerable<Person> LeaderList { get; }

        public Material Material { get; private set; }

        public string Notes { get; set; }

        public Person Requester
        {
            get { return _requester; }
            set
            {
                _requester = value;
                RaisePropertyChanged();
            }
        }

        public IEnumerable<ReportItemWrapper> RequirementList { get; private set; }

        public Batch SelectedBatch
        {
            get { return _selectedBatch; }
            set
            {
                _selectedBatch = value;
                Material = _selectedBatch.Material;

                if (Material != null && Material.ExternalConstruction != null)
                {
                    SpecificationVersion tempVersion = _labDbData.RunQuery(new SpecificationVersionQuery() { ID = Material.ExternalConstruction.DefaultSpecVersionID });
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

                Notes = (_selectedControlPlan != null) ? _selectedControlPlan.Name : "";

                RaisePropertyChanged("SelectedControlPlan");
                if (value != null && RequirementList != null)
                    _reportService.ApplyControlPlan(RequirementList, _selectedControlPlan);
            }
        }

        public Specification SelectedSpecification
        {
            get { return _selectedSpecification; }
            set
            {
                _selectedSpecification = value;
                ControlPlanList = _selectedSpecification.GetControlPlans();
                VersionList = _selectedSpecification.GetVersions();

                RaisePropertyChanged("ControlPlanList");
                RaisePropertyChanged("IsSpecificationSelected");
                RaisePropertyChanged("SelectedSpecification");
                RaisePropertyChanged("VersionList");

                SelectedControlPlan = ControlPlanList.First(cp => cp.IsDefault);
                SelectedVersion = VersionList.First(sv => sv.IsMain);
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
                    RequirementList = _selectedVersion.GenerateRequirementList()
                                                        .Select(req => new ReportItemWrapper(req, this))
                                                        .ToList();
                    _reportService.ApplyControlPlan(RequirementList, _selectedControlPlan);
                }
                else
                    RequirementList = new List<ReportItemWrapper>();

                RaisePropertyChanged("RequirementList");
                RaisePropertyChanged("SelectedVersion");
            }
        }

        public IEnumerable<Specification> SpecificationList { get; }

        public Task TaskInstance { get; private set; }

        public double TotalDuration => (RequirementList != null) ? RequirementList.Where(req => req.IsSelected)
                                                                                    .Sum(req => req.Duration) : 0;

        public IList<SpecificationVersion> VersionList { get; private set; }

        #endregion Properties

        #region Methods

        public void OnRequirementSelectionChanged()
        {
            RaisePropertyChanged("TotalDuration");
        }

        #endregion Methods
    }
}