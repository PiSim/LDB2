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
        private Material _selectedAssigned, _selectedUnassigned;
        private Batch _selectedBatch;
        private DBPrincipal _principal;
        private DelegateCommand _assignMaterial, 
                                _openBatch, 
                                _openExternalReport, 
                                _openReport, 
                                _save,
                                _startEdit,
                                _unassignMaterial;
        private EventAggregator _eventAggregator;
        private ExternalReport _selectedExternal;
        private Project _projectInstance;
        private Report _selectedReport;

        public ProjectInfoViewModel(DBPrincipal principal,
                                    EventAggregator aggregator)
            : base()
        {
            _editMode = false;
            _eventAggregator = aggregator;
            _principal = principal;

            #region EventSubscriptions
            
            _eventAggregator.GetEvent<ReportListUpdateRequested>().Subscribe(
                () =>
                {
                    SelectedReport = null;
                    RaisePropertyChanged("ReportList");
                }); 

            _eventAggregator.GetEvent<TaskListUpdateRequested>().Subscribe(() => RaisePropertyChanged("TaskList"));

            #endregion

            #region CommandDefinitions

            _assignMaterial = new DelegateCommand(
                () => 
                {
                    _selectedUnassigned.ProjectID = _projectInstance.ID;
                    _selectedUnassigned.Update();

                    SelectedUnassigned = null;
                    RaisePropertyChanged("AssignedMaterials");
                    RaisePropertyChanged("UnassignedMaterials");
                },
                () => _selectedUnassigned != null
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
                () => !_editMode);

            _unassignMaterial = new DelegateCommand(
                () => 
                {
                    _selectedAssigned.ProjectID = null;
                    _selectedAssigned.Update();
                    
                    RaisePropertyChanged("AssignedMaterials");
                    RaisePropertyChanged("UnassignedMaterials");
                    SelectedAssigned = null;
                },
                () => _selectedAssigned != null
            );

            #endregion
        }

        public DelegateCommand AssignMaterialCommand
        {
            get { return _assignMaterial; }
        }
        
        public IEnumerable<Material> AssignedMaterials
        {
            get { return _projectInstance.GetMaterials(); }
            private set
            {
                RaisePropertyChanged("AssignedMaterials");
            }
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

                AssignedMaterials = new ObservableCollection<Material>(_projectInstance.Materials);

                UnassignedMaterials = new ObservableCollection<Material>(MaterialService.GetMaterialsWithoutProject());
                
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

        public DelegateCommand SaveCommand
        {
            get { return _save; }
        }

        public Material SelectedAssigned
        {
            get { return _selectedAssigned; }
            set 
            { 
                _selectedAssigned = value; 
                RaisePropertyChanged("SelectedAssigned");
                _unassignMaterial.RaiseCanExecuteChanged();
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
        
        public Material SelectedUnassigned
        {
            get { return _selectedUnassigned; }
            set 
            { 
                _selectedUnassigned = value; 
                RaisePropertyChanged("SelectedUnassigned");
                _assignMaterial.RaiseCanExecuteChanged();
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
        
        public DelegateCommand UnassignMaterialCommand
        {
            get { return _unassignMaterial; }
        }
        
        public IEnumerable<Material> UnassignedMaterials
        {
            get { return DataService.GetMaterialsWithoutProject(); }
            private set
            {
                RaisePropertyChanged("UnassignedMaterials");
            }
        }
    }        
}