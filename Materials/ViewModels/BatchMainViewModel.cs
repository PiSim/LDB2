using DBManager;
using DBManager.Services;
using Infrastructure;
using Infrastructure.Events;
using Navigation;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Reporting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Materials.ViewModels
{
    class BatchMainViewModel : BindableBase
    {
        private DBPrincipal _principal;
        private DelegateCommand _createBatch, 
                                _quickOpen, 
                                _openSampleLogView,
                                _printStatusList,
                                _refresh;
        private IEventAggregator _eventAggregator;
        private Sample _selectedSampleArrival;
        private string _batchNumber;

        public BatchMainViewModel(DBPrincipal principal,
                                IEventAggregator eventAggregator) 
            : base()
        {
            _eventAggregator = eventAggregator;
            _principal = principal;

            _createBatch = new DelegateCommand(
                () =>
                {
                    _eventAggregator.GetEvent<BatchCreationRequested>()
                                    .Publish();
                },
                () => CanCreateBatch);

            _openSampleLogView = new DelegateCommand(
                () =>
                {
                    _eventAggregator.GetEvent<NavigationRequested>().Publish(new NavigationToken(MaterialViewNames.SampleLogView));
                });
            
            _quickOpen = new DelegateCommand(
                () =>
                {
                    _eventAggregator.GetEvent<BatchVisualizationRequested>()
                                    .Publish(_batchNumber);
                });

            _printStatusList = new DelegateCommand(
                () =>
                {
                    ReportingEngine.PrintBatchStatusList();
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

        public string BatchStatusListRegionName
        {
            get { return RegionNames.BatchStatusListRegion; }
        }

        private bool CanCreateBatch
        {
            get { return _principal.IsInRole(UserRoleNames.MaterialAdmin); }
        }

        public DelegateCommand CreateBatchCommand
        {
            get { return _createBatch; }
        }

        public DelegateCommand OpenSampleLogViewCommand
        {
            get { return _openSampleLogView; }
        }

        public DelegateCommand QuickOpenCommand
        {
            get { return _quickOpen; }
        }

        public DelegateCommand PrintStatusListCommand
        {
            get { return _printStatusList; }
        }

        public IEnumerable<Sample> RecentArrivalsList
        {
            get
            {
                return MaterialService.GetRecentlyArrivedSamples();
            }
        }

        public DelegateCommand RefreshCommand
        {
            get
            {
                return _refresh;
            }
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
