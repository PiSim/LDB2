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
    public class SampleArchiveViewModel : BindableBase
    {
        private DelegateCommand<DataGrid> _openBatch;
        private EventAggregator _eventAggregator;
        private IDataService _dataService;

        public SampleArchiveViewModel(EventAggregator eventAggregator,
                                        IDataService dataService) : base()
        {
            _dataService = dataService;
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


        public IEnumerable<Batch> BatchList => _dataService.GetArchive();

        public DelegateCommand<DataGrid> OpenBatchCommand
        {
            get { return _openBatch; }
        }
    }
}
