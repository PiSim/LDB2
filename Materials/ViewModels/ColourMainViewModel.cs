using DBManager;
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

        public ColourMainViewModel(DBEntities entities) : base()
        {
            _entities = entities;
        }   

        public List<Colour> ColourList
        {
            get { return new List<Colour>(_entities.Colours); }
        }

        public Colour SelectedColour
        {
            get { return _selectedColour; }
            set
            {
                _selectedColour = value;
            }
        }
    }
}
