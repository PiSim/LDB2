using DBManager;
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
    public class ConstructionDetailViewModel : BindableBase
    {
        private Construction _constructionInstance;
        private EventAggregator _eventAggregator;

        public ConstructionDetailViewModel(EventAggregator eventAggregator) : base()
        {
            _eventAggregator = eventAggregator;
        }

        public IEnumerable<Batch> BatchList
        {
            get
            {
                return _constructionInstance.GetBatches();
            }
        }

        public string ConstructionBatchListRegionName
        {
            get { return RegionNames.ConstructionDetailBatchListRegion; }
        }

        public Construction ConstructionInstance
        {
            get { return _constructionInstance; }
            set
            {
                _constructionInstance = value;

                RaisePropertyChanged();
                RaisePropertyChanged("BatchList");
            }
        }


    }
}
