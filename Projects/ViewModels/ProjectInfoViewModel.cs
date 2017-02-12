using DBManager;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projects.ViewModels
{
    internal class ProjectInfoViewModel : BindableBase
    {
        private DBEntities _entities;
        private DelegateCommand _openExternal, _openReport;
        private EventAggregator _eventAggregator;
        private ExternalReport _selectedExternal;
        private Project _projectInstance;
        private Report _selectedReport;

        internal ProjectInfoViewModel(DBEntities entities,
                                    EventAggregator aggregator,
                                    Project instance)
            : base()
        {
            _entities = entities;
            _eventAggregator = aggregator;
            _projectInstance = instance;

            _openExternal = new DelegateCommand(
                () =>
                {

                });

            _openReport = new DelegateCommand(
                () =>
                {

                });
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

        public List<DBManager.Task> TaskList
        {
            get { return new List<DBManager.Task>(_projectInstance.Tasks); }
        }
    }        
}