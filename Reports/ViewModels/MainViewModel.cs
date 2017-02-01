using DBManager;
using Infrastructure;
using Infrastructure.Events;
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
    class MainViewModel : BindableBase
    {
        private DBEntities _entities;
        private DelegateCommand _newReport, _openReport;
        private EventAggregator _eventAggregator;
        private ObservableCollection<Report> _reportList;
        private Report _selectedReport;

        public MainViewModel(DBEntities entities, EventAggregator eventAggregator) : base()
        {
            _entities = entities;
            _eventAggregator = eventAggregator;
            _reportList = new ObservableCollection<Report>(entities.Reports);

            _openReport = new DelegateCommand(
                () =>
                {
                    ObjectNavigationToken token = new ObjectNavigationToken(SelectedReport, ViewNames.ReportEditView);
                    _eventAggregator.GetEvent<VisualizeObjectRequested>().Publish(token);
                });

            _newReport = new DelegateCommand(
                () =>
                {
                    
                });
        }

        public DelegateCommand OpenReportCommand
        {
            get { return _openReport; }
        }

        public ObservableCollection<Report> ReportList
        {
            get { return _reportList; }
        }

        public Report SelectedReport
        {
            get { return _selectedReport; }
            set { _selectedReport = value; }
        }
    }
}
