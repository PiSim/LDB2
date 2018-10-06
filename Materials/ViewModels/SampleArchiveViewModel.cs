using DataAccess;
using Infrastructure;
using Infrastructure.Events;
using LabDbContext;
using Materials.Queries;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;

namespace Materials.ViewModels
{
    public class SampleArchiveViewModel : BindableBase
    {
        #region Fields

        private IEventAggregator _eventAggregator;
        private IDataService<LabDbEntities> _labDbData;

        #endregion Fields

        #region Constructors

        public SampleArchiveViewModel(IEventAggregator eventAggregator,
                                        IDataService<LabDbEntities> labDbData) : base()
        {
            _labDbData = labDbData;
            _eventAggregator = eventAggregator;

            OpenBatchCommand = new DelegateCommand<DataGrid>(
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

        #endregion Constructors

        #region Properties

        public IEnumerable<Batch> BatchList => _labDbData.RunQuery(new SampleArchiveQuery())
                                                        .ToList();

        public DelegateCommand<DataGrid> OpenBatchCommand { get; }

        #endregion Properties
    }
}