using DBManager;
using DBManager.Services;
using Infrastructure;
using Infrastructure.Events;
using Navigation;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Materials.ViewModels
{
    class BatchMainViewModel : BindableBase
    {
        private DelegateCommand _createBatch, _quickOpen, _openSampleLogView;
        private IEventAggregator _eventAggregator;
        private Sample _selectedSampleArrival;
        private string _batchNumber;

        public BatchMainViewModel(IEventAggregator eventAggregator) 
            : base()
        {
            _eventAggregator = eventAggregator;

            _createBatch = new DelegateCommand(
                () =>
                {
                    _eventAggregator.GetEvent<BatchCreationRequested>()
                                    .Publish();
                });

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

            _eventAggregator.GetEvent<SampleListUpdateRequested>().Subscribe(
                () =>
                {
                    RaisePropertyChanged("RecentArrivalsList");
                    SelectedSampleArrival = null;
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

        public IEnumerable<Sample> RecentArrivalsList
        {
            get
            {
                return MaterialService.GetRecentlyArrivedSamples();
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
