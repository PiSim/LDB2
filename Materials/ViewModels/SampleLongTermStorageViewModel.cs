using DBManager;
using DBManager.EntityExtensions;
using DBManager.Services;
using Infrastructure;
using Infrastructure.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Materials.ViewModels
{
    public class SampleLongTermStorageViewModel : BindableBase
    {
        private DelegateCommand<DataGrid> _openBatch;
        private EventAggregator _eventAggregator;

        public SampleLongTermStorageViewModel(EventAggregator eventAggregator) : base()
        {
            _eventAggregator = eventAggregator;

            _openBatch = new DelegateCommand<DataGrid>(
                grid =>
                {
                    Batch btc = grid.SelectedItem as Batch;
                    NavigationToken token = new NavigationToken(MaterialViewNames.BatchInfoView,
                                                                btc);

                    _eventAggregator.GetEvent<NavigationRequested>()
                                    .Publish(token);
                });


            _eventAggregator.GetEvent<SampleLogCreated>()
                .Subscribe(
                sample =>
                {
                    RaisePropertyChanged("BatchList");
                });

            _eventAggregator.GetEvent<BatchStatusListRefreshRequested>()
                            .Subscribe(
                            () =>
                            {
                                RaisePropertyChanged("BatchList");
                            });
        }


        public IEnumerable<Batch> BatchList
        {
            get { return DBManager.Services.MaterialService.GetLongTermStorage(); }
        }

        public DelegateCommand<DataGrid> OpenBatchCommand
        {
            get { return _openBatch; }
        }
    }
}
