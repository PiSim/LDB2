using DBManager;
using Infrastructure;
using Infrastructure.Events;
using Infrastructure.Tokens;
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
        private Construction _selectedAssigned, _selectedUnassigned;
        private Batch _selectedBatch;
        private DBEntities _entities;
        private DBPrincipal _principal;
        private DelegateCommand _assignConstruction, _openBatch, _modifyDetails, _newReport, 
            _openExternalReport, _openReport, _removeReport, _unassignConstruction;
        private EventAggregator _eventAggregator;
        private ExternalReport _selectedExternal;
        private IProjectServiceProvider _projectServiceProvider;
        private IUnityContainer _container;
        private ObservableCollection<Construction> _assignedConstructions, _unassignedConstructions;
        private Project _projectInstance;
        private Report _selectedReport;

        public ProjectInfoViewModel(DBEntities entities,
                                    DBPrincipal principal,
                                    EventAggregator aggregator,
                                    IProjectServiceProvider projectServiceProvider,
                                    IUnityContainer container)
            : base()
        {
            _entities = entities;
            _container = container;
            _eventAggregator = aggregator;
            _principal = principal;
            _projectServiceProvider = projectServiceProvider;

            #region EventSubscriptions

            _eventAggregator.GetEvent<CommitRequested>().Subscribe(() => _entities.SaveChanges());
            _eventAggregator.GetEvent<ReportListUpdateRequested>().Subscribe(
                () => 
                {
                    RaisePropertyChanged("ReportList");
                    SelectedReport = null;
                }); 

            #endregion

            #region CommandDefinitions

            _assignConstruction = new DelegateCommand(
                () => 
                {
                    AssignedConstructions.Add(_selectedUnassigned);
                    _projectInstance.Constructions.Add(_selectedUnassigned);
                    UnassignedConstructions.Remove(_selectedUnassigned);
                    SelectedUnassigned = null;
                },
                () => _selectedUnassigned != null
            );

            _modifyDetails = new DelegateCommand(
                () =>
                {
                    _projectServiceProvider.AlterProjectInfo(_projectInstance);
                    RaisePropertyChanged("LeaderName");
                    RaisePropertyChanged("Name");
                    RaisePropertyChanged("OemName");
                });

            _newReport = new DelegateCommand(
                () =>
                {
                    NewReportToken token = new NewReportToken();
                    _eventAggregator.GetEvent<ReportCreationRequested>().Publish(token);
                }
            );

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

            _removeReport = new DelegateCommand(
                () =>
                {
                    _entities.Reports.Remove(_selectedReport);
                    _eventAggregator.GetEvent<ReportListUpdateRequested>().Publish();
                },
                () => CanRemoveReport && SelectedReport != null);
                
            _unassignConstruction = new DelegateCommand(
                () => 
                {
                    UnassignedConstructions.Add(_selectedAssigned);
                    _projectInstance.Constructions.Remove(_selectedAssigned);
                    AssignedConstructions.Remove(_selectedAssigned);
                    SelectedAssigned = null;
                },
                () => _selectedAssigned != null
            );

            #endregion
        }

        public DelegateCommand AssignConstructionCommand
        {
            get { return _assignConstruction; }
        }
        
        public ObservableCollection<Construction> AssignedConstructions
        {
            get { return _assignedConstructions; }
            private set
            {
                _assignedConstructions = value;
                RaisePropertyChanged("AssignedConstructions");
            }
        }

        public List<Batch> BatchList
        {
            get 
            { 
                if (_projectInstance == null)
                    return null;
                    
                return new List<Batch>(_entities.Batches.Where(btc => btc.Material.Construction.ProjectID == _projectInstance.ID));
            }
        }

        public bool CanCreateReport
        {
            get
            {
                return _principal.IsInRole(UserRoleNames.ReportEdit);
            }
        }

        public bool CanRemoveReport
        {
            get
            {
                if (SelectedReport == null)
                    return false;
                
                else if (_principal.IsInRole.(UserRoleNames.ReportAdmin))
                    return true;
                    
                else
                    return _principal.IsInRole(UserRoleNames.ReportEdit)
                            && SelectedReport.Author.ID == _principal.CurrentPerson.ID;
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
        }

        public List<ExternalReport> ExternalReportList
        {
            get
            {
                if (_projectInstance == null)
                    return null;

                return new List<ExternalReport>
                    (_entities.ExternalReports.Where(ext => ext.ProjectID == _projectInstance.ID));
            }
        }
        
        public string LeaderName
        {
            get
            {
                if (_projectInstance == null)
                    return null;

                return _projectInstance.Leader.Name;
            }
        }

        public DelegateCommand ModifyDetailsCommand
        {
            get { return _modifyDetails; }
        }
        
        public string Name
        {
            get
            {
                if (_projectInstance == null)
                    return null;

                return _projectInstance.Name;
            }
        }
        
        public DelegateCommand NewReportCommand
        {
            get { return _newReport; }
        }

        public string OemName
        {
            get
            {
                if (_projectInstance == null)
                    return null;

                return _projectInstance.Oem.Name;
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

                AssignedConstructions = new ObservableCollection<Construction>(_projectInstance.Constructions);

                UnassignedConstructions = new ObservableCollection<Construction>(
                   _entities.Constructions.Where(cns => cns.Project == null));
                
                SelectedBatch = null;

                RaisePropertyChanged("BatchList");
                RaisePropertyChanged("Description");
                RaisePropertyChanged("ExternalReportList");
                RaisePropertyChanged("LeaderName");
                RaisePropertyChanged("Name");
                RaisePropertyChanged("OemName");
                RaisePropertyChanged("ReportList");
                RaisePropertyChanged("TaskList");
            }
        }

        public DelegateCommand RemoveReportCommand
        {
            get { return _removeReport; }
        }

        public List<Report> ReportList
        {
            get
            {
                return new List<Report>(_entities.Reports
                    .Where(rpt => rpt.Batch.Material.Construction.ProjectID == _projectInstance.ID));
            }
        }

        public Construction SelectedAssigned
        {
            get { return _selectedAssigned; }
            set 
            { 
                _selectedAssigned = value; 
                RaisePropertyChanged("SelectedAssigned");
                _unassignConstruction.RaiseCanExecuteChanged();
            }
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

        public Report SelectedReport
        {
            get { return _selectedReport; }
            set 
            { 
                _selectedReport = value; 
                _openReport.RaiseCanExecuteChanged();
                _removeReport.RaiseCanExecuteChanged(),
            }
        }
        
        public Construction SelectedUnassigned
        {
            get { return _selectedUnassigned; }
            set 
            { 
                _selectedUnassigned = value; 
                RaisePropertyChanged("SelectedUnassigned");
                _assignConstruction.RaiseCanExecuteChanged();
            }
        }

        public List<DBManager.Task> TaskList
        {
            get
            {
                if (_projectInstance == null)
                    return null;

                return new List<DBManager.Task>(_projectInstance.Tasks);
            }
        }
        
        public DelegateCommand UnassignConstructionCommand
        {
            get { return _unassignConstruction; }
        }
        
        public ObservableCollection<Construction> UnassignedConstructions
        {
            get { return _unassignedConstructions; }
            private set
            {
                _unassignedConstructions = value;
                RaisePropertyChanged("UnassignedConstructions");
            }
        }
    }        
}