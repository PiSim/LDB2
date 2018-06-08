using DBManager;
using Controls.Views;
using Infrastructure;
using Infrastructure.Events;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBManager.Services;
using DBManager.EntityExtensions;

namespace Reports.ViewModels
{
    public class ReportMainViewModel : BindableBase
    {
        private DBPrincipal _principal;
        private DelegateCommand _newReport, _openReport, _removeReport;
        private EventAggregator _eventAggregator;
        private IDataService _dataService;
        private IReportService _reportService;
        private Report _selectedReport;

        public ReportMainViewModel(DBPrincipal principal,
                                    EventAggregator eventAggregator,
                                    IDataService dataService,
                                    IReportService reportService)
        {
            _dataService = dataService;
            _eventAggregator = eventAggregator;
            _principal = principal;
            _reportService = reportService;

            _openReport = new DelegateCommand(
                () =>
                {
                    NavigationToken token = new NavigationToken(ViewNames.ReportEditView, SelectedReport);
                    _eventAggregator.GetEvent<NavigationRequested>().Publish(token);
                },
                () => SelectedReport != null);

            _newReport = new DelegateCommand(
                () => _reportService.CreateReport(),
                () => CanCreateReport);

            _removeReport = new DelegateCommand(
                () =>
                {
                    _selectedReport.Delete();
                    _eventAggregator.GetEvent<ReportDeleted>().Publish(_selectedReport);
                },
                () => CanRemoveReport && SelectedReport != null);

            _eventAggregator.GetEvent<ReportCreated>().Subscribe(
                report => 
                {
                    RaisePropertyChanged("ReportList");
                    SelectedReport = null;
                });

            _eventAggregator.GetEvent<ReportDeleted>().Subscribe(
                report =>
                {
                    RaisePropertyChanged("ReportList");
                    SelectedReport = null;
                });

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
                    
                else
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

        public DelegateCommand RemoveReportCommand
        {
            get { return _removeReport; }
        }

        public IEnumerable<Report> ReportList => _dataService.GetReports();

        public Report SelectedReport
        {
            get { return _selectedReport; }
            set 
            { 
                _selectedReport = value; 
                OpenReportCommand.RaiseCanExecuteChanged();
                RemoveReportCommand.RaiseCanExecuteChanged();
                RaisePropertyChanged("SelectedReport");                
            }
        }
    }
}
