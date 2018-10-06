using DataAccess;
using Infrastructure;
using Infrastructure.Events;
using Infrastructure.Queries;
using LabDbContext;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Reports.ViewModels
{
    public class ExternalReportMainViewModel : BindableBase
    {
        #region Fields

        private IEventAggregator _eventAggregator;
        private IDataService<LabDbEntities> _labDbData;
        private IReportService _reportService;
        private ExternalReport _selectedReport;

        #endregion Fields

        #region Constructors

        public ExternalReportMainViewModel(IDataService<LabDbEntities> labDbData,
                                            IEventAggregator aggregator,
                                            IReportService reportService)
        {
            _labDbData = labDbData;
            _eventAggregator = aggregator;
            _reportService = reportService;

            NewExternalReportCommand = new DelegateCommand(
                () =>
                {
                    _reportService.CreateExternalReport();
                },
                () => Thread.CurrentPrincipal.IsInRole(UserRoleNames.ReportEdit));

            OpenExternalReportCommand = new DelegateCommand(
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

        #endregion Constructors

        #region Properties

        public bool CanCreateExternalReport
        {
            get
            {
                if (Thread.CurrentPrincipal.IsInRole(UserRoleNames.ExternalReportEdit))
                    return true;
                else
                    return false;
            }
        }

        public IEnumerable<ExternalReport> ExternalReportList => _labDbData.RunQuery(new ExternalReportsQuery())
                                                                            .ToList();

        public DelegateCommand NewExternalReportCommand { get; }

        public DelegateCommand OpenExternalReportCommand { get; }

        public ExternalReport SelectedExternalReport
        {
            get { return _selectedReport; }
            set
            {
                _selectedReport = value;
                OpenExternalReportCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion Properties
    }
}