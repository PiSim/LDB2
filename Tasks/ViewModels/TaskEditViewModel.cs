using DBManager;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tasks.ViewModels
{
    public class TaskEditViewModel : BindableBase
    {
        private DBEntities _entities;
        private DelegateCommand _convert;
        private EventAggregator _eventAggregator;

        public TaskEditViewModel(DBEntities entities,
                                EventAggregator aggregator) : base()
        {
            _entities = entities;
            _eventAggregator = aggregator;
        }


    }
}
