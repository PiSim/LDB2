using Controls.Views;
using DataAccess;
using LabDbContext;
using Materials.Queries;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Linq;

namespace Materials.ViewModels
{
    public class MaterialDetailViewModel : BindableBase
    {
        #region Fields

        private IEventAggregator _eventAggregator;
        private IDataService<LabDbEntities> _labDbData;
        private Material _materialInstance;

        #endregion Fields

        #region Constructors

        public MaterialDetailViewModel(IDataService<LabDbEntities> labDbData, IEventAggregator eventAggregator) : base()
        {
            _eventAggregator = eventAggregator;
            _labDbData = labDbData;
        }

        #endregion Constructors

        #region Properties

        public IEnumerable<Batch> BatchList => (_materialInstance == null) ? null : _labDbData.RunQuery(new BatchesQuery() { })
                                                                                            .Where(btc => btc.MaterialID == _materialInstance.ID)
                                                                                            .ToList();

        public string MaterialBatchListRegionName => RegionNames.MaterialDetailBatchListRegion;

        public Material MaterialInstance
        {
            get { return _materialInstance; }
            set
            {
                _materialInstance = value;

                RaisePropertyChanged("MaterialInstance");
                RaisePropertyChanged("BatchList");
            }
        }

        #endregion Properties
    }
}