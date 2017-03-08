using DBManager;
using Infrastructure.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace Specifications.ViewModels
{
    public class MethodEditViewModel : BindableBase
    {
        private DBEntities _entities;
        private EventAggregator _eventAggregator;
        private Method _methodInstance;

        public MethodEditViewModel(DBEntities entities, EventAggregator aggregator) : base()
        {
            _entities = entities;
            _eventAggregator = aggregator;

            _eventAggregator.GetEvent<CommitRequested>().Subscribe(() => _entities.SaveChanges());
        }

        public Method MethodInstance
        {
            get { return _methodInstance; }
            set
            {
                _methodInstance = value;
            }
        }

        public string Name
        {
            get
            {
                return (_methodInstance != null) ? _methodInstance.Standard.Name : null;
            }
        }
    }
}
