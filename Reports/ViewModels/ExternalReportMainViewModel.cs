using DBManager;
using DBManager.Services;
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

namespace Reports.ViewModels
{
    public class ExternalReportMainViewModel : BindableBase
    {
        private DBPrincipal _principal;
        private DelegateCommand _newReport, _openReport;
        private EventAggregator _eventAggregator;
        private ExternalReport _selectedReport;
        private IDataService _dataService;
        private IReportService _reportService;

        public ExternalReportMainViewModel(DBPrincipal principal,
                                            EventAggregator aggregator,
                                            IDataService dataService,
                                            IReportService reportService)
        {
            _dataService = dataService;
            _eventAggregator = aggregator;
            _reportService = reportService;
            _principal = principal;
            
            _newReport = new DelegateCommand(
                () => 
                {
                    _reportService.CreateExternalReport();
                }, 
                () => _principal.IsInRole(UserRoleNames.ReportEdit));

            _openReport = new DelegateCommand(
                () =>
                {
                    NavigationToken token = new NavigationToken(ViewNames.ExternalReportEditView,
                                                                _selectedReport);

                    _eventAggregator.GetEvent<NavigationRequested>().Publish(token);
                },
                () => _selectedReport != null);

            _eventAggregator.GetEvent<ExternalReportChanged>()
                            .Subscribe(
                            extr =>
                            {
                                SelectedExternalReport = null;
                                RaisePropertyChanged("ExternalReportList");
                            });
        } 

        public bool CanCreateExternalReport
        {
            get 
            {
                if (_principal.IsInRole(UserRoleNames.ExternalReportEdit))
                    return true;
                
                else 
                    return false;
            }
        }
        
        public DelegateCommand NewExternalReportCommand
        {
            get { return _newReport; }
        }

        public DelegateCommand OpenExternalReportCommand
        {
            get { return _openReport; }
        }

        public IEnumerable<ExternalReport> ExternalReportList => _dataService.GetExternalReports();

        public ExternalReport SelectedExternalReport
        {
            get { return _selectedReport; }
            set 
            { 
                _selectedReport = value; 
                _openReport.RaiseCanExecuteChanged();
            }
        }
    }
}
