using DBManager;
using Infrastructure;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reports.ViewModels
{
    internal class ExternalReportMainViewModel : BindableBase
    {
        private DBEntities _entities;
        private DelegateCommand _newReport, _openReport;
        private EventAggregator _eventAggregator;
        private ExternalReport _selectedReport;
        private ObservableCollection<ExternalReport> _reportList;       

        internal ExternalReportMainViewModel(DBEntities entities, 
                                            EventAggregator aggregator)
        {
            _entities = entities;
            _eventAggregator = aggregator;
            _reportList = new ObservableCollection<ExternalReport>(_entities.ExternalReports);
            
            _newReport = new DelegateCommand(
                () => 
                {
                    
                });

            _openReport = new DelegateCommand(
                () =>
                {
                    ObjectNavigationToken token = 
                        new ObjectNavigationToken(_selectedReport, ViewNames.ExternalReportEditView);

                    _eventAggregator.GetEvent<Infrastructure.Events.VisualizeObjectRequested>()
                        .Publish(token);
                });
        } 
        
        public DelegateCommand NewReportCommand
        {
            get { return _newReport; }
        }

        public DelegateCommand OpenReportCommand
        {
            get { return _openReport; }
        }

        public ObservableCollection<ExternalReport> ReportList
        {
            get { return _reportList; }
        }

        public ExternalReport SelectedReport
        {
            get { return _selectedReport; }
            set { _selectedReport = value; }
        }
    }
}
