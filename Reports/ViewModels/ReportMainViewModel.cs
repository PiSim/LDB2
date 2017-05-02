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

namespace Reports.ViewModels
{
    class ReportMainViewModel : BindableBase
    {
        private DBEntities _entities;
        private DBPrincipal _principal;
        private DelegateCommand _newReport, _openReport, _removeReport;
        private EventAggregator _eventAggregator;
        private Report _selectedReport;

        public ReportMainViewModel(DBEntities entities, 
                            DBPrincipal principal,
                            EventAggregator eventAggregator) : base()
        {
            _entities = entities;
            _eventAggregator = eventAggregator;
            _principal = principal;

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

            _removeReport = new DelegateCommand(
                () =>
                {
                    _entities.Reports.Remove(_selectedReport);
                    _eventAggregator.GetEvent<ReportListUpdateRequested>().Publish();
                },
                () => CanRemoveReport && SelectedReport != null);

            _eventAggregator.GetEvent<ReportListUpdateRequested>().Subscribe(
                () => 
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
                
                else if (_principal.IsInRole(UserRoleNames.ReportAdmin))
                    return true;
                    
                else
                    return _principal.IsInRole(UserRoleNames.ReportEdit)
                            && SelectedReport.Author.ID == _principal.CurrentPerson.ID;
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

        public List<Report> ReportList
        {
            get { return new List<Report>(_entities.Reports); }
        }

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
