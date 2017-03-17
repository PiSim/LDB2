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
                    _eventAggregator.GetEvent<NavigationRequested>().Publish(new NavigationToken(ViewNames.SampleLogView));
                });
            
            _quickOpen = new DelegateCommand(
                () =>
                {
                    Batch temp = _materialServiceProvider.GetBatch(_batchNumber);
                    NavigationToken token = new NavigationToken(ViewNames.BatchInfoView, temp);
                    if (temp != null)
                        _eventAggregator.GetEvent<NavigationRequested>().Publish(token);
                });
        }

        public DelegateCommand OpenSampleLogViewCommand
        {
            get { return _openSampleLogView; }
        }

        public DelegateCommand QuickOpenCommand
        {
            get { return _quickOpen; }
        }

        public string BatchNumber
        {
            get { return _batchNumber; }
            set
            {
                _batchNumber = value;
                OnPropertyChanged("BatchNumber");
            }
        }
    }
}
