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

namespace Batches.ViewModels
{
    class BatchMainViewModel : BindableBase
    {
        private DBEntities _entities;
        private DelegateCommand _quickOpen, _openSampleLogView;
        private IEventAggregator _eventAggregator;
        private string _batchNumber;

        public BatchMainViewModel(IEventAggregator eventAggregator, DBEntities entities) 
            : base()
        {
            _entities = entities;
            _eventAggregator = eventAggregator;

            _openSampleLogView = new DelegateCommand(
                () =>
                {
                    _eventAggregator.GetEvent<NavigationRequested>().Publish(ViewNames.SampleLogView);
                });
            
            _quickOpen = new DelegateCommand(
                () =>
                {
                    Batch temp = _entities.GetBatchByNumber(_batchNumber, false);
                    ObjectNavigationToken token = new ObjectNavigationToken(temp, ViewNames.BatchInfoView);
                    if (temp != null)
                        _eventAggregator.GetEvent<VisualizeObjectRequested>().Publish(token);
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
