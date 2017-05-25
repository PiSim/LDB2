using DBManager.Services;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    internal class ServiceEventListener
    {
        private EventAggregator _eventAggregator;

        public ServiceEventListener(EventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;


        }
    }
}
