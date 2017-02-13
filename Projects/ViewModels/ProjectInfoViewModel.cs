using DBManager;
using Infrastructure;
using Infrastructure.Events;
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
    internal class ProjectInfoViewModel : BindableBase
    {
        private Construction _selectedAssigned, _selectedUnassigned;
        private DBEntities _entities;
        private DelegateCommand _assignConstruction, _openExternal, _openReport, _unassignConstruction;
        private EventAggregator _eventAggregator;
        private ExternalReport _selectedExternal;
        private ObservableCollection<Construction> _assignedConstructions, _unassignedConstructions;
        private Project _projectInstance;
        private Report _selectedReport;

        internal ProjectInfoViewModel(DBEntities entities,
                                    EventAggregator aggregator,
                                    Project instance)
            : base()
        {
            _entities = entities;
            _projectInstance = instance;
            
            _eventAggregator = aggregator;
            _eventAggregator.GetEvent<CommitRequested>().Subscribe( () => _entities.SaveChanges() );
            
            _assignedConstructions = new ObservableCollection<Construction>(_projectInstance.Constructions);
                
            _unassignedConstructions = new ObservableCollection<Construction>(
               _entities.Constructions.Where(cns => cns.Project == null));

            _assignConstruction = new DelegateCommand(
                () => 
                {
                    AssignedConstructions.Add(_selectedUnassigned);
                    UnassignedConstructions.Remove(_selectedUnassigned);
                    _projectInstance.Constructions.Add(_selectedUnassigned);
                    SelectedUnassigned = null;
                },
                () => _selectedUnassigned != null
            );

            _openExternal = new DelegateCommand(
                () =>
                {
                    
                });

            _openReport = new DelegateCommand(
                () =>
                {
                    ObjectNavigationToken token = new ObjectNavigationToken(_selectedReport, Reports.ViewNames.ReportEditView);
                    _eventAggregator.GetEvent<VisualizeObjectRequested>().Publish(token);
                });
                
            _unassignConstruction = new DelegateCommand(
                () => 
                {
                    UnassignedConstructions.Add(_selectedAssigned);
                    AssignedConstructions.Remove(_selectedAssigned);
                    _projectInstance.Constructions.Remove(_selectedAssigned);
                    SelectedAssigned = null;
                },
                () => _selectedAssigned != null
            );
        }
        
        public DelegateCommand AssignConstructionCommand
        {
            get { return _assignConstruction; }
        }
        
        public ObservableCollection<Construction> AssignedConstructions
        {
            get { return _assignedConstructions; }
        }

        public List<ExternalReport> ExternalReportList
        {
            get
            {
                return new List<ExternalReport>
                    (_entities.ExternalReports.Where(ext => ext.projectID == _projectInstance.ID));
            }
        }

        public DelegateCommand OpenExternalCommand
        {
            get { return _openExternal; }
        }

        public DelegateCommand OpenReportCommand
        {
            get { return _openReport; }
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
                OnPropertyChanged("SelectedAssigned");
                _unassignConstruction.RaiseCanExecuteChanged();
            }
        }

        public ExternalReport SelectedExternal
        {
            get { return _selectedExternal; }
            set { _selectedExternal = value; }
        }

        public Report SelectedReport
        {
            get { return _selectedReport; }
            set { _selectedReport = value; }
        }
        
        public Construction SelectedUnassigned
        {
            get { return _selectedUnassigned; }
            set 
            { 
                _selectedUnassigned = value; 
                OnPropertyChanged("SelectedUnassigned");
                _assignConstruction.RaiseCanExecuteChanged();
            }
        }

        public List<DBManager.Task> TaskList
        {
            get { return new List<DBManager.Task>(_projectInstance.Tasks); }
        }
        
        public DelegateCommand UnassignConstructionCommand
        {
            get { return _unassignConstruction; }
        }
        
        public ObservableCollection<Construction> UnassignedConstructions
        {
            get { return _unassignedConstructions; }
        }
    }        
}