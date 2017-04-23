using DBManager;
using Controls.Views;
using Infrastructure;
using Infrastructure.Events;
using Infrastructure.Tokens;
using Microsoft.Practices.Unity;
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
    class ReportMainViewModel : BindableBase
    {
        private DBEntities _entities;
        private DBPrincipal _principal;
        private DelegateCommand _newReport, _openReport;
        private EventAggregator _eventAggregator;
        private ObservableCollection<Report> _reportList;
        private Report _selectedReport;

        public ReportMainViewModel(DBEntities entities, 
                            DBPrincipal principal,
                            EventAggregator eventAggregator) : base()
        {
            _entities = entities;
            _eventAggregator = eventAggregator;
            _principal = principal;
            _reportList = new ObservableCollection<Report>(entities.Reports);

            _openReport = new DelegateCommand(
                () =>
                {
                    NavigationToken token = new NavigationToken(ViewNames.ReportEditView, SelectedReport);
                    _eventAggregator.GetEvent<NavigationRequested>().Publish(token);
                },
                () => SelectedReport != null);

            _newReport = new DelegateCommand(
                () =>
                {
                    _eventAggregator.GetEvent<ReportCreationRequested>().Publish(new NewReportToken());
                },
                () => CanCreateReport);

            _eventAggregator.GetEvent<ReportCreated>().Subscribe(
                rpt => 
                {
                    _reportList.Add(rpt);
                    SelectedReport = rpt;
                } ); 
        }

        public bool CanCreateReport
        {
            get
            {
                return _principal.IsInRole(UserRoleNames.ReportEdit);
            }
        }
            
        public DelegateCommand NewReportCommand
        {
            get { return _newReport; }
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
            set 
            { 
                _selectedReport = value; 
                OpenReportCommand.RaiseCanExecuteChanged();
                RaisePropertyChanged("SelectedReport");                
            }
        }
    }
}
