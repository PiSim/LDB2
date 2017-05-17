using DBManager;
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
    public class AspectDetailViewModel : BindableBase
    {
        private Aspect _aspectInstance;
        private DBEntities _entities;
        private EventAggregator _eventAggregator;

        public AspectDetailViewModel(DBEntities entities,
                                    EventAggregator eventAggregator) : base()
        {
            _entities = entities;
            _eventAggregator = eventAggregator;
        }

        public static string AspectDetailBatchListRegionName
        {
            get { return RegionNames.AspectDetailBatchListRegion; }
        }

        public Aspect AspectInstance
        {
            get { return _aspectInstance; }
            set
            {
                _aspectInstance = _entities.Aspects.First(asp => asp.ID == value.ID);
                RaisePropertyChanged();

                RaisePropertyChanged("BatchList");
            }
        }

        public List<Batch> BatchList
        {
            get
            {
                if (_aspectInstance == null)
                    return new List<Batch>();

                else
                    return new List<Batch>(_entities.Batches.Where(btc => btc.Material.Construction.Aspect.ID == _aspectInstance.ID));
            }
        }
    }
}
