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
        DBEntities _entities;
        DelegateCommand _quickOpen, _openSampleLogView;
        IEventAggregator _eventAggregator;
        string _batchNumber;

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
                    if (_entities != null)
                        _eventAggregator.GetEvent<VisualizeObjectRequested>().Publish();
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

    }
}
