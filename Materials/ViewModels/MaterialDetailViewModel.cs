using DBManager;
using DBManager.EntityExtensions;
using DBManager.Services;
using Infrastructure;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Materials.ViewModels
{
    public class MaterialDetailViewModel : BindableBase
    {
        private Material _materialInstance;
        private EventAggregator _eventAggregator;

        public MaterialDetailViewModel(EventAggregator eventAggregator) : base()
        {
            _eventAggregator = eventAggregator;
        }

        public IEnumerable<Batch> BatchList
        {
            get
            {
                return _materialInstance.GetBatches();
            }
        }

        public string MaterialBatchListRegionName
        {
            get { return RegionNames.MaterialDetailBatchListRegion; }
        }

        public Material MaterialInstance
        {
            get { return _materialInstance; }
            set
            {
                _materialInstance = value;

                RaisePropertyChanged();
                RaisePropertyChanged("BatchList");
            }
        }


    }
}
