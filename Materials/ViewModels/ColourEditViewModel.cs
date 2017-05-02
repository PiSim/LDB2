using DBManager;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Materials.ViewModels
{
    public class ColourEditViewModel :BindableBase
    {
        private Colour _colourInstance;
        private DBEntities _entities;

        public ColourEditViewModel(DBEntities entities) : base()
        {
            _entities = entities;
        }

        public Colour ColourInstance
        {
            get { return _colourInstance; }
            set
            {
                if (value != null)
                    _colourInstance = _entities.Colours.First(clr => clr.ID == value.ID);
                
                else
                    _colourInstance = value;
                
                RaisePropertyChanged("ColourInstance"); 
            }
        }
    }
}
