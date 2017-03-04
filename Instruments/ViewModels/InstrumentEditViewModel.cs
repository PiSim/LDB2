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

namespace Instruments.ViewModels
{
    public class InstrumentEditViewModel : BindableBase
    {
        private DBEntities _entities;
        private EventAggregator _eventAggregator;
        private Instrument _instance;

        public InstrumentEditViewModel(DBEntities entities, EventAggregator aggregator) : base()
        {
            _entities = entities;
            _eventAggregator = aggregator;

            _eventAggregator.GetEvent<CommitRequested>().Subscribe(() => _entities.SaveChanges());
        }

        public Instrument InstrumentInstance
        {
            get { return _instance; }
            set
            {
                _instance = value;
            }
        }

        public string Code
        {
            get { return _instance.Code; }
        }
    }
}
