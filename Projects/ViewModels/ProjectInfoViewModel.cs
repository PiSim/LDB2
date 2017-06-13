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
        private Construction _selectedAssigned, _selectedUnassigned;
        private Batch _selectedBatch;
        private DBPrincipal _principal;
        private DelegateCommand _assignConstruction, _openBatch, _modifyDetails, 
            _openExternalReport, _openReport, _unassignConstruction;
        private EventAggregator _eventAggregator;
        private ExternalReport _selectedExternal;
        private Project _projectInstance;
        private Report _selectedReport;

        public ProjectInfoViewModel(DBPrincipal principal,
                                    EventAggregator aggregator)
            : base()
        {
            _eventAggregator = aggregator;
            _principal = principal;

            #region EventSubscriptions

            _eventAggregator.GetEvent<CommitRequested>().Subscribe
                (() =>
                {
                    _projectInstance.Update();
                });
            
            _eventAggregator.GetEvent<ReportListUpdateRequested>().Subscribe(
                () =>
                {
                    SelectedReport = null;
                    RaisePropertyChanged("ReportList");
                }); 

            _eventAggregator.GetEvent<TaskListUpdateRequested>().Subscribe(() => RaisePropertyChanged("TaskList"));

            #endregion

            #region CommandDefinitions

            _assignConstruction = new DelegateCommand(
                () => 
                {
                    _selectedUnassigned.ProjectID = _projectInstance.ID;
                    _selectedUnassigned.Update();

                    SelectedUnassigned = null;
                    RaisePropertyChanged("AssignedConstructions");
                    RaisePropertyChanged("UnassignedConstructions");
                },
                () => _selectedUnassigned != null
            );

            _modifyDetails = new DelegateCommand(
                () =>
                {
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
                
            _unassignConstruction = new DelegateCommand(
                () => 
                {
                    _selectedAssigned.ProjectID = null;
                    _selectedAssigned.Update();
                    
                    RaisePropertyChanged("AssignedConstructions");
                    RaisePropertyChanged("UnassignedConstructions");
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
        
        public IEnumerable<Construction> AssignedConstructions
        {
            get { return _projectInstance.GetConstructions(); }
            private set
            {
                RaisePropertyChanged("AssignedConstructions");
            }
        }

        public IEnumerable<Batch> BatchList
        {
            get 
            {
                return _projectInstance.GetBatches();
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
                
                else if (_principal.IsInRole(UserRoleNames.ReportAdmin))
                    return true;
                    
                else
                    return (_principal.IsInRole(UserRoleNames.ReportEdit)
                            && (SelectedReport.Author.ID == _principal.CurrentPerson.ID));
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
                _projectInstance.Load();

                AssignedConstructions = new ObservableCollection<Construction>(_projectInstance.Constructions);

                UnassignedConstructions = new ObservableCollection<Construction>(MaterialService.GetConstructionsWithoutProject());
                
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

        public IEnumerable<Report> ReportList
        {
            get
            {
                return _projectInstance.GetReports();
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

        public IEnumerable<DBManager.Task> TaskList
        {
            get
            {
                if (_projectInstance == null)
                    return null;

                return _projectInstance.GetTasks();
            }
        }
        
        public DelegateCommand UnassignConstructionCommand
        {
            get { return _unassignConstruction; }
        }
        
        public IEnumerable<Construction> UnassignedConstructions
        {
            get { return DataService.GetConstructionsWithoutProject(); }
            private set
            {
                RaisePropertyChanged("UnassignedConstructions");
            }
        }
    }        
}