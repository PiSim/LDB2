using DBManager;
using DBManager.Services;
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
    public class ConstructionMainViewModel : BindableBase
    {
        private Construction _selectedConstruction;
        private EventAggregator _eventAggregator;

        public ConstructionMainViewModel(EventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }

        public string ConstructionDetailRegionName
        {
            get { return RegionNames.ConstructionDetailRegion; }
        }

        public IEnumerable<Construction> ConstructionList
        {
            get { return MaterialService.GetConstructions(); }
        }

        public Construction SelectedConstruction
        {
            get { return _selectedConstruction; }
            set
            {
                _selectedConstruction = value;
                RaisePropertyChanged();

                NavigationToken token = new NavigationToken(MaterialViewNames.ConstructionDetail,
                                                            _selectedConstruction,
                                                            ConstructionDetailRegionName);

                _eventAggregator.GetEvent<NavigationRequested>().Publish(token);
            }
        }

    }
}
