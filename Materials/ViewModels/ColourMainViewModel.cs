using DBManager;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Materials.ViewModels
{
    public class ColourMainViewModel : BindableBase
    {
        private Colour _selectedColour;
        private DBEntities _entities;
        private EventAggregator _eventAggregator;

        public ColourMainViewModel(DBEntities entities,
                                    EventAggregator eventAggregator) : base()
        {
            _entities = entities;
            _eventAggregator = eventAggregator;
        }   

        public string ColourEditRegionName
        {
            get { return RegionNames.ColourEditRegion; }
        }

        public List<Colour> ColourList
        {
            get { return new List<Colour>(_entities.Colours); }
        }

        public Colour SelectedColour
        {
            get
            {
                return _selectedColour;
            }

            set
            {
                _selectedColour = value;
                RaisePropertyChanged("SelectedColour");
                NavigationToken token = new NavigationToken(ViewNames.ColourEditView,
                                                            _selectedColour,
                                                            RegionNames.ColourEditRegion);

                _eventAggregator.GetEvent<NavigationRequested>().Publish(token);
            }
        }
    }
}
