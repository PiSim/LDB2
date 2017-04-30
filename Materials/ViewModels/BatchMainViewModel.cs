using DBManager;
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
        private DBEntities _entities;
        private DelegateCommand _quickOpen, _openSampleLogView;
        private IEventAggregator _eventAggregator;
        private IMaterialServiceProvider _materialServiceProvider;
        private Sample _selectedSampleArrival;
        private string _batchNumber;

        public BatchMainViewModel(IEventAggregator eventAggregator, 
                                IMaterialServiceProvider serviceProvider,
                                DBEntities entities) 
            : base()
        {
            _entities = entities;
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
                    Batch temp = _entities.Batches.FirstOrDefault(btc => btc.Number == _batchNumber);

                    if (temp != null)
                    {
                        NavigationToken token = new NavigationToken(MaterialViewNames.BatchInfoView, temp);
                        _eventAggregator.GetEvent<NavigationRequested>().Publish(token);
                    }

                    else
                        _eventAggregator.GetEvent<StatusNotificationIssued>().Publish("Batch non trovato");
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

        public List<Sample> RecentArrivalsList
        {
            get
            {
                return new List<Sample>(_entities.Samples.Where(sle => sle.Code== "A")
                                                        .OrderByDescending(sle => sle.Date)
                                                        .Take(25));
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
