using DBManager;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tasks.ViewModels
{
    internal class TaskMainViewModel : BindableBase
    {
        private DBEntities _entities;
        private EventAggregator _eventAggregator;

        internal TaskMainViewModel(DBEntities entities, EventAggregator eventAggregator) 
            : base()
        {
            _entities = entities;
            _eventAggregator = eventAggregator;
        }
    }
}
