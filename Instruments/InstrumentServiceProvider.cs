using DBManager;
using Infrastructure;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instruments
{
    public class InstrumentServiceProvider : IInstrumentServiceProvider
    {
        private DBEntities _entities;
        private EventAggregator _eventAggregator;

        public InstrumentServiceProvider(DBEntities entities,
                                        EventAggregator aggregator)
        {
            _entities = entities;
            _eventAggregator = aggregator;
        }

        public Instrument RegisterNewInstrument()
        {

        }


    }
}
