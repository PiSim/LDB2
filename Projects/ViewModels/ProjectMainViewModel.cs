using DBManager;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projects.ViewModels
{
    internal class ProjectMainViewModel : BindableBase
    {
        private DBEntities _entities;
        private EventAggregator _eventAggregator;

        internal ProjectMainViewModel(DBEntities entities, EventAggregator aggregator) 
            : base()
        {
            _entities = entities;
            _eventAggregator = aggregator;
        }
    }
}
