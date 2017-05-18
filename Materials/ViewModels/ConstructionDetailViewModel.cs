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
    public class ConstructionDetailViewModel : BindableBase
    {
        private Construction _constructionInstance;
        private DBEntities _entities;
        private EventAggregator _eventAggregator;

        public ConstructionDetailViewModel(DBEntities entities,
                                            EventAggregator eventAggregator) : base()
        {
            _entities = entities;
            _eventAggregator = eventAggregator;
        }

        public List<Batch> BatchList
        {
            get
            {
                if (_constructionInstance == null)
                    return new List<Batch>();

                else
                    return new List<Batch>(_entities.Batches.Where(btc => btc.Material.Construction.ID == _constructionInstance.ID));
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
                _constructionInstance = _entities.Constructions.First(cns => cns.ID == value.ID);
                RaisePropertyChanged();

                RaisePropertyChanged("BatchList");
            }
        }


    }
}
