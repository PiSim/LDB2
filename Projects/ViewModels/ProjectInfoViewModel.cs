using Controls.Views;
using DataAccess;
using Infrastructure;
using Infrastructure.Events;
using Infrastructure.Queries;
using LabDbContext;
using LabDbContext.EntityExtensions;
using LabDbContext.Services;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Projects.ViewModels
{
    public class ProjectInfoViewModel : BindableBase
    {
        #region Fields

        private bool _editMode;
        private IEventAggregator _eventAggregator;
        private IDataService<LabDbEntities> _labDbData;
        private IEnumerable<Person> _leaderList;
        private IEnumerable<Organization> _oemList;
        private Project _projectInstance;
        private DelegateCommand _save;
        private Batch _selectedBatch;
        private ExternalReport _selectedExternal;
        private Report _selectedReport;

        #endregion Fields

        #region Constructors

        public ProjectInfoViewModel(IDataService<LabDbEntities> labDbData,
                                    IEventAggregator aggregator)
            : base()
        {
            _labDbData = labDbData;
            _editMode = false;
            _eventAggregator = aggregator;

            #region EventSubscriptions

            // ReportList Update

            _eventAggregator.GetEvent<ReportCreated>().Subscribe(
                report =>
                {
                    SelectedReport = null;
                    RaisePropertyChanged("ReportList");
                });

            // LeaderList Update

            _eventAggregator.GetEvent<PersonChanged>()
                            .Subscribe(ect => UpdateLeaderList());

            // OEMList Update

            _eventAggregator.GetEvent<OrganizationChanged>()
                            .Subscribe(ect => UpdateOEMList());

            // TaskList Update

            _eventAggregator.GetEvent<TaskListUpdateRequested>()
                            .Subscribe(() => RaisePropertyChanged("TaskList"));

            #endregion EventSubscriptions

            #region CommandDefinitions

            AddMaterialCommand = new DelegateCommand<Material>(
                mat =>
                {
                    if (mat != null)
                    {
                        mat.ProjectID = _projectInstance.ID;
                        mat.Update();

                        RaisePropertyChanged("AssignedMaterials");
                        RaisePropertyChanged("UnassignedMaterials");
                    }
                });

            OpenBatchCommand = new DelegateCommand(
                () =>
                {
                    NavigationToken token = new NavigationToken(MaterialViewNames.BatchInfoView,
                                                                SelectedBatch);
                    _eventAggregator.GetEvent<NavigationRequested>().Publish(token);
                },
                () => SelectedBatch != null);

            OpenExternalReportCommand = new DelegateCommand(
                () =>
                {
                    NavigationToken token = new NavigationToken(Reports.ViewNames.ExternalReportEditView,
                                                                SelectedExternalReport);
                    _eventAggregator.GetEvent<NavigationRequested>().Publish(token);
                },
                () => SelectedExternalReport != null);

            OpenReportCommand = new DelegateCommand(
                () =>
                {
                    NavigationToken token = new NavigationToken(Reports.ViewNames.ReportEditView,
                                                                _selectedReport);
                    _eventAggregator.GetEvent<NavigationRequested>().Publish(token);
                });

            RemoveMaterialCommand = new DelegateCommand<Material>(
                mat =>
                {
                    if (mat != null)
                    {
                        mat.ProjectID = null;
                        mat.Update();

                        RaisePropertyChanged("AssignedMaterials");
                        RaisePropertyChanged("UnassignedMaterials");
                    }
                });

            _save = new DelegateCommand(
                () =>
                {
                    _projectInstance.Update();
                    EditMode = false;
                },
                () => _editMode);

            StartEditCommand = new DelegateCommand(
                () =>
                {
                    EditMode = true;
                },
                () => !_editMode && Thread.CurrentPrincipal.IsInRole(UserRoleNames.ProjectAdmin));

            #endregion CommandDefinitions
        }

        #endregion Constructors

        #region Methods

        private void UpdateLeaderList()
        {
            _leaderList = null;
            RaisePropertyChanged("LeaderList");
            RaisePropertyChanged("SelectedLeader");
        }

        private void UpdateOEMList()
        {
            _oemList = null;
            RaisePropertyChanged("OEMList");
            RaisePropertyChanged("SelectedOEM");
        }

        #endregion Methods

        #region Properties

        public DelegateCommand<Material> AddMaterialCommand { get; }

        public IEnumerable<Material> AssignedMaterials => _projectInstance.GetMaterials();

        public IEnumerable<Batch> BatchList => _projectInstance.GetBatches();

        public string Description
        {
            get
            {
                if (_projectInstance == null)
                    return null;

                return _projectInstance.Description;
            }

            set
            {
                _projectInstance.Description = value;
            }
        }

        public bool EditMode
        {
            get { return _editMode; }
            set
            {
                _editMode = value;
                RaisePropertyChanged("EditMode");

                _save.RaiseCanExecuteChanged();
                StartEditCommand.RaiseCanExecuteChanged();
            }
        }

        public IEnumerable<ExternalReport> ExternalReportList
        {
            get
            {
                if (_projectInstance == null)
                    return null;

                return _projectInstance.ExternalReports;
            }
        }

        public IEnumerable<Person> LeaderList
        {
            get
            {
                if (_leaderList == null)
                    _leaderList = _labDbData.RunQuery(new PeopleQuery() { Role = PeopleQuery.PersonRoles.ProjectLeader })
                                                            .ToList();

                return _leaderList;
            }
        }

        public string LeaderName
        {
            get
            {
                if (_projectInstance == null || _projectInstance.Leader == null)
                    return null;

                return _projectInstance.Leader.Name;
            }
        }

        public string Name
        {
            get
            {
                if (_projectInstance == null)
                    return null;

                return _projectInstance.Name;
            }
            set
            {
                _projectInstance.Name = value;
            }
        }

        public IEnumerable<Organization> OEMList
        {
            get
            {
                if (_oemList == null)
                    _oemList = _labDbData.RunQuery(new OrganizationsQuery() { Role = OrganizationsQuery.OrganizationRoles.OEM })
                                                                        .ToList(); ;

                return _oemList;
            }
        }

        public DelegateCommand OpenBatchCommand { get; }

        public DelegateCommand OpenExternalReportCommand { get; }

        public DelegateCommand OpenReportCommand { get; }

        public string ProjectExternalReportListRegionName => RegionNames.ProjectExternalReportListRegion;

        public Project ProjectInstance
        {
            get { return _projectInstance; }
            set
            {
                _projectInstance = value;
                _projectInstance?.Load();

                SelectedBatch = null;

                RaisePropertyChanged("AssignedMaterials");
                RaisePropertyChanged("BatchList");
                RaisePropertyChanged("Description");
                RaisePropertyChanged("ExternalReportList");
                RaisePropertyChanged("Name");
                RaisePropertyChanged("ReportList");
                RaisePropertyChanged("SelectedLeader");
                RaisePropertyChanged("SelectedOEM");
                RaisePropertyChanged("TaskList");
                RaisePropertyChanged("UnassignedMaterials");
            }
        }

        public DelegateCommand<Material> RemoveMaterialCommand { get; }

        public IEnumerable<Report> ReportList => _projectInstance.GetReports();

        public DelegateCommand SaveCommand => _save;

        public Batch SelectedBatch
        {
            get { return _selectedBatch; }
            set
            {
                _selectedBatch = value;
                OpenBatchCommand.RaiseCanExecuteChanged();
                RaisePropertyChanged("SelectedBatch");
            }
        }

        public ExternalReport SelectedExternalReport
        {
            get { return _selectedExternal; }
            set
            {
                _selectedExternal = value;
                OpenExternalReportCommand.RaiseCanExecuteChanged();
            }
        }

        public Person SelectedLeader
        {
            get
            {
                return _leaderList?.FirstOrDefault(pjl => pjl.ID == _projectInstance?.ProjectLeaderID);
            }

            set
            {
                _projectInstance.ProjectLeaderID = value.ID;
            }
        }

        public Organization SelectedOEM
        {
            get
            {
                return _oemList?.FirstOrDefault(oem => oem.ID == _projectInstance?.OemID);
            }

            set
            {
                _projectInstance.OemID = value.ID;
            }
        }

        public Report SelectedReport
        {
            get { return _selectedReport; }
            set
            {
                _selectedReport = value;
                OpenReportCommand.RaiseCanExecuteChanged();
            }
        }

        public DelegateCommand StartEditCommand { get; }

        public IEnumerable<LabDbContext.Task> TaskList
        {
            get
            {
                if (_projectInstance == null)
                    return null;

                return _projectInstance.GetTasks();
            }
        }

        public IEnumerable<Material> UnassignedMaterials => _labDbData.RunQuery(new MaterialsQuery())
                                                                .Where(mat => mat.ProjectID == null)
                                                                .ToList();

        #endregion Properties
    }
}