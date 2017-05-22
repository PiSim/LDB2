﻿using DBManager;
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

        public ExternalReportMainViewModel(DBPrincipal principal,
                                            EventAggregator aggregator)
        {
            _eventAggregator = aggregator;
            _principal = principal;
            
            _newReport = new DelegateCommand(
                () => 
                {
                    _eventAggregator.GetEvent<ExternalReportCreationRequested>()
                                    .Publish();
                }, 
                () => _principal.IsInRole(UserRoleNames.ReportAdmin));

            _openReport = new DelegateCommand(
                () =>
                {
                    NavigationToken token = new NavigationToken(ViewNames.ExternalReportEditView,
                                                                _selectedReport);

                    _eventAggregator.GetEvent<NavigationRequested>().Publish(token);
                },
                () => _selectedReport != null);
        } 

        public bool CanCreateExternalReport
        {
            get 
            {
                if (_principal.IsInRole(UserRoleNames.ExternalReportAdmin))
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

        public IEnumerable<ExternalReport> ExternalReportList
        {
            get { return ReportService.GetExternalReports(); }
        }

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
