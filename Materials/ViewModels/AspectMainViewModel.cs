using DBManager;
using Infrastructure;
using Infrastructure.Events;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Materials.ViewModels
{
    public class AspectMainViewModel : BindableBase
    {
        private Aspect _selectedAspect;
        private DBEntities _entities;
        private EventAggregator _eventAggregator;

        public AspectMainViewModel(DBEntities entities,
                                    EventAggregator eventAggregator) : base()
        {
            _entities = entities;
            _eventAggregator = eventAggregator;
        }

        public string AspectDetailRegionName
        {
            get { return RegionNames.AspectDetailRegion; }
        }

        public List<Aspect> AspectList
        {
            get { return new List<Aspect>(_entities.Aspects); }
        }

        public Aspect SelectedAspect
        {
            get { return _selectedAspect; }
            set
            {
                _selectedAspect = value;
                RaisePropertyChanged();
                NavigationToken token = new NavigationToken(MaterialViewNames.AspectDetail,
                                                            _selectedAspect,
                                                            AspectDetailRegionName);

                _eventAggregator.GetEvent<NavigationRequested>().Publish(token);
            }
        }
    }
}
