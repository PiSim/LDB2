using DBManager;
using DBManager.EntityExtensions;
using DBManager.Services;
using Infrastructure;
using Infrastructure.Events;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projects.ViewModels
{
    public class ProjectInfoViewModel : BindableBase
    {
        private bool _editMode;
        private Batch _selectedBatch;
        private DBPrincipal _principal;
        private DelegateCommand _openBatch, 
                                _openExternalReport, 
                                _openReport, 
                                _save,
                                _startEdit;
        private DelegateCommand<Material> _addMaterial,
                                        _removeMaterial;
        private IDataService _dataService;
        private IEnumerable<Person> _leaderList;
        private IEnumerable<Organization> _oemList;
        private EventAggregator _eventAggregator;
        private ExternalReport _selectedExternal;
        private Project _projectInstance;
        private Report _selectedReport;

        public ProjectInfoViewModel(DBPrincipal principal,
                                    EventAggregator aggregator,
                                    IDataService dataService)
            : base()
        {
            _dataService = dataService;
            _editMode = false;
            _eventAggregator = aggregator;
            _principal = principal;

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

            #endregion

            #region CommandDefinitions

            _addMaterial = new DelegateCommand<Material>(
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
            
            _openBatch = new DelegateCommand(
                () => 
                {
                    NavigationToken token = new NavigationToken(MaterialViewNames.BatchInfoView,
                                                                SelectedBatch);
                    _eventAggregator.GetEvent<NavigationRequested>().Publish(token);
                },
                () => SelectedBatch != null);

            _openExternalReport = new DelegateCommand(
                () =>
                {
                    NavigationToken token = new NavigationToken(Reports.ViewNames.ExternalReportEditView,
                                                                SelectedExternal);
                    _eventAggregator.GetEvent<NavigationRequested>().Publish(token);
                },
                () => SelectedExternal != null);

            _openReport = new DelegateCommand(
                () =>
                {
                    NavigationToken token = new NavigationToken(Reports.ViewNames.ReportEditView,
                                                                _selectedReport);
                    _eventAggregator.GetEvent<NavigationRequested>().Publish(token);
                });

            _removeMaterial = new DelegateCommand<Material>(
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

            _startEdit = new DelegateCommand(
                () =>
                {
                    EditMode = true;
                },
                () => !_editMode && _principal.IsInRole(UserRoleNames.ProjectAdmin));

            #endregion
        }

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

        #endregion

        public DelegateCommand<Material> AddMaterialCommand => _addMaterial;
        
        public IEnumerable<Material> AssignedMaterials
        {
            get { return _projectInstance.GetMaterials(); }
        }

        public IEnumerable<Batch> BatchList
        {
            get 
            {
                return _projectInstance.GetBatches();
            }
        }

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
                _startEdit.RaiseCanExecuteChanged();
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
        
        public string LeaderName
        {
            get
            {
                if (_projectInstance == null || _projectInstance.Leader == null)
                    return null;

                return _projectInstance.Leader.Name;
            }
        }

        public IEnumerable<Person> LeaderList
        {
            get
            {
                if (_leaderList == null)
                    _leaderList = _dataService.GetPeople(PersonRoleNames.ProjectLeader);

                return _leaderList;
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
                    _oemList = _dataService.GetOrganizations(OrganizationRoleNames.OEM);

                return _oemList;
            }
        }

        public DelegateCommand OpenBatchCommand
        {
            get { return _openBatch; }
        }

        public DelegateCommand OpenExternalReportCommand
        {
            get { return _openExternalReport; }
        }

        public DelegateCommand OpenReportCommand
        {
            get { return _openReport; }
        }

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

        public DelegateCommand<Material> RemoveMaterialCommand => _removeMaterial;

        public IEnumerable<Report> ReportList
        {
            get
            {
                return _projectInstance.GetReports();
            }
        }

        public DelegateCommand SaveCommand
        {
            get { return _save; }
        }
        
        public Batch SelectedBatch
        {
            get { return _selectedBatch; }
            set
            {
                _selectedBatch = value;
                _openBatch.RaiseCanExecuteChanged();
                RaisePropertyChanged("SelectedBatch");
            }
        }

        public ExternalReport SelectedExternal
        {
            get { return _selectedExternal; }
            set
            {
                _selectedExternal = value;
                _openExternalReport.RaiseCanExecuteChanged();
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
                _openReport.RaiseCanExecuteChanged();
            }
        }
        public DelegateCommand StartEditCommand
        {
            get { return _startEdit; }
        }

        public IEnumerable<DBManager.Task> TaskList
        {
            get
            {
                if (_projectInstance == null)
                    return null;

                return _projectInstance.GetTasks();
            }
        }
        
        public IEnumerable<Material> UnassignedMaterials
        {
            get { return MaterialService.GetMaterialsWithoutProject(); }
        }
    }        
}