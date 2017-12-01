using DBManager;
using DBManager.Services;
using Infrastructure;
using Infrastructure.Events;
using Infrastructure.Queries;
using Infrastructure.Queries.Presentation;
using Navigation;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Reporting;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Materials.ViewModels
{
    class BatchMainViewModel : BindableBase
    {
        private bool _isPrintMenuOpen;
        private DBPrincipal _principal;
        private DelegateCommand _createBatch, 
                                _quickOpen, 
                                _openPrintMenu,
                                _openSampleLogView,
                                _printStatusList,
                                _refresh;
        private DelegateCommand<IQueryPresenter<Batch>> _printBatchQuery;
        private IDataService _dataService;
        private IEventAggregator _eventAggregator;
        private IMaterialService _materialService;
        private IReportingService _reportingService;
        private Sample _selectedSampleArrival;
        private string _batchNumber;

        public BatchMainViewModel(DBPrincipal principal,
                                IEventAggregator eventAggregator,
                                IDataService dataService,
                                IMaterialService materialService,
                                IReportingService reportingService) 
            : base()
        {
            _dataService = dataService;
            _eventAggregator = eventAggregator;
            _isPrintMenuOpen = false;
            _materialService = materialService;
            _principal = principal;
            _reportingService = reportingService;

            _createBatch = new DelegateCommand(
                () =>
                {
                    _eventAggregator.GetEvent<BatchCreationRequested>()
                                    .Publish();
                },
                () => _principal.IsInRole(UserRoleNames.BatchEdit));

            _openPrintMenu = new DelegateCommand(
                () =>
                {
                    IsPrintMenuOpen = true;
                });

            _openSampleLogView = new DelegateCommand(
                () =>
                {
                    CommonProcedures.StartSampleLog();
                },
                () => _principal.IsInRole(UserRoleNames.SampleEdit));
            
            _quickOpen = new DelegateCommand(
                () =>
                {
                    _eventAggregator.GetEvent<BatchVisualizationRequested>()
                                    .Publish(_batchNumber);
                });

            _printBatchQuery = new DelegateCommand<IQueryPresenter<Batch>>(
                query =>
                {
                    IEnumerable<Batch> tempQuery = _dataService.GetQueryResults(query.Query);
                    _reportingService.PrintBatchReport(tempQuery);
                });

            _printStatusList = new DelegateCommand(
                () =>
                {
                    _reportingService.PrintBatchReport(_dataService.GetBatches(50));
                });

            _refresh = new DelegateCommand(
                () =>
                {
                    _eventAggregator.GetEvent<BatchStatusListRefreshRequested>()
                                    .Publish();
                });
        }

        public string BatchNumber
        {
            get { return _batchNumber; }
            set
            {
                _batchNumber = value;
                RaisePropertyChanged("BatchNumber");
            }
        }

        public IEnumerable<IQueryPresenter<Batch>> QueryList => _materialService.GetBatchQueries();

        public string BatchStatusListRegionName
        {
            get { return RegionNames.BatchStatusListRegion; }
        }

        public DelegateCommand CreateBatchCommand
        {
            get { return _createBatch; }
        }

        public bool IsPrintMenuOpen
        {
            get => _isPrintMenuOpen;
            set
            {
                _isPrintMenuOpen = value;
                RaisePropertyChanged();
            }
        }

        public DelegateCommand OpenPrintMenuCommand => _openPrintMenu;

        public DelegateCommand OpenSampleLogViewCommand
        {
            get { return _openSampleLogView; }
        }

        public DelegateCommand QuickOpenCommand
        {
            get { return _quickOpen; }
        }

        public DelegateCommand<IQueryPresenter<Batch>> PrintBatchQueryCommand => _printBatchQuery;

        public DelegateCommand PrintStatusListCommand
        {
            get { return _printStatusList; }
        }

        public IEnumerable<Sample> RecentArrivalsList
        {
            get
            {
                return DBManager.Services.MaterialService.GetLatestSamples();
            }
        }

        public DelegateCommand RefreshCommand
        {
            get
            {
                return _refresh;
            }
        }

        public string SampleArchiveRegionName
        {
            get { return RegionNames.SampleArchiveRegion; }
        }

        public string SampleLongTermStorageRegionName
        {
            get { return RegionNames.SampleLongTermStorageRegion; }
        }

        public Sample SelectedSampleArrival
        {
            get { return _selectedSampleArrival; }
            set
            {
                _selectedSampleArrival = value;
            }
        }
    }
}
