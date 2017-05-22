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
        private DelegateCommand _quickOpen, _openSampleLogView;
        private IEventAggregator _eventAggregator;
        private IMaterialServiceProvider _materialServiceProvider;
        private Sample _selectedSampleArrival;
        private string _batchNumber;

        public BatchMainViewModel(IEventAggregator eventAggregator, 
                                IMaterialServiceProvider serviceProvider) 
            : base()
        {
            _eventAggregator = eventAggregator;
            _materialServiceProvider = serviceProvider;

            _openSampleLogView = new DelegateCommand(
                () =>
                {
                    _eventAggregator.GetEvent<NavigationRequested>().Publish(new NavigationToken(MaterialViewNames.SampleLogView));
                });
            
            _quickOpen = new DelegateCommand(
                () =>
                {
                    _materialServiceProvider.TryQuickBatchVisualize(_batchNumber);
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
