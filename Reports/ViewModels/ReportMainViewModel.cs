using DataAccess;
using Infrastructure;
using Infrastructure.Events;
using Infrastructure.Queries;
using LabDbContext;
using LabDbContext.EntityExtensions;
using LabDbContext.Services;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Reports.Commands;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Reports.ViewModels
{
    public class ReportMainViewModel : BindableBase
    {
        #region Fields

        private IEventAggregator _eventAggregator;
        private IDataService<LabDbEntities> _labDbData;
        private IReportService _reportService;
        private Report _selectedReport;

        #endregion Fields

        #region Constructors

        public ReportMainViewModel(IDataService<LabDbEntities> labDbData,
                                    IEventAggregator eventAggregator,
                                    IReportService reportService)
        {
            _labDbData = labDbData;
            _eventAggregator = eventAggregator;
            _reportService = reportService;

            OpenReportCommand = new DelegateCommand<Report>(
                rep =>
                {
                    NavigationToken token = new NavigationToken(ViewNames.ReportEditView, rep);
                    _eventAggregator.GetEvent<NavigationRequested>().Publish(token);
                },
                rep => rep != null);

            NewReportCommand = new DelegateCommand(
                () => _reportService.CreateReport(),
                () => CanCreateReport);

            RemoveReportCommand = new DelegateCommand(
                () =>
                {
                    _selectedReport.Delete();
                    _eventAggregator.GetEvent<ReportDeleted>().Publish(_selectedReport);
                },
                () => CanRemoveReport && SelectedReport != null);


            SetReportsCompleteCommand = new DelegateCommand<IList>(
                replist =>
                {
                    _labDbData.Execute(new SetReportsIsCompleteCommand(replist.Cast<Report>(), true));
                    RaisePropertyChanged("ReportList");
                },
                replist => Thread.CurrentPrincipal.IsInRole(UserRoleNames.ReportEdit));

            SetReportsNotCompleteCommand = new DelegateCommand<IList>(
                replist =>
                {
                    _labDbData.Execute(new SetReportsIsCompleteCommand(replist.Cast<Report>(), false));
                    RaisePropertyChanged("ReportList");
                });

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

        #endregion Constructors

        #region Properties

        public bool CanCreateReport => Thread.CurrentPrincipal.IsInRole(UserRoleNames.ReportEdit);

        public bool CanRemoveReport
        {
            get
            {
                if (SelectedReport == null)
                    return false;
                else
                    return Thread.CurrentPrincipal.IsInRole(UserRoleNames.ReportEdit);
            }
        }

        public DelegateCommand NewReportCommand { get; }
        public DelegateCommand<Report> OpenReportCommand { get; }
        public DelegateCommand RemoveReportCommand { get; }
        public IEnumerable<Report> ReportList => _labDbData.RunQuery(new ReportsQuery()).ToList();
        public DelegateCommand<Report> RowDoubleClickCommand { get; }

        public Report SelectedReport
        {
            get { return _selectedReport; }
            set
            {
                _selectedReport = value;
                RemoveReportCommand.RaiseCanExecuteChanged();
                RaisePropertyChanged("SelectedReport");
            }
        }

        public DelegateCommand<IList> SetReportsCompleteCommand { get; }

        public DelegateCommand<IList> SetReportsNotCompleteCommand { get; }

        #endregion Properties
    }
}