using DBManager;
using DBManager.EntityExtensions;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.ViewModels
{
    public class PropertyEditViewModel : BindableBase
    {
        private EventAggregator _eventAggregator;
        private Property _propertyInstance;

        public PropertyEditViewModel(EventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
        }
        
        public Property PropertyInstance
        {
            get { return _propertyInstance; }
            set
            {
                _propertyInstance = value;
            }
        }
        
    }
}
