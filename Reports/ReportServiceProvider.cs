using Infrastructure.Events;
using Infrastructure.Tokens;
using Microsoft.Practices.Unity;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reports
{
    public class ReportServiceProvider
    {
        private EventAggregator _eventAggregator;
        private IUnityContainer _container;

        public ReportServiceProvider(EventAggregator aggregator,
                                    IUnityContainer container)
        {
            _container = container;
            _eventAggregator = aggregator;

            _eventAggregator.GetEvent<ReportCreationRequested>().Subscribe(
                token =>
                {
                    Views.ReportCreationDialog creationDialog =
                        _container.Resolve<Views.ReportCreationDialog>();

                    if (creationDialog.ShowDialog() == true)
                    {
                        _eventAggregator.GetEvent<ReportCreated>().Publish(creationDialog.ReportInstance);
                    }
                });
        }
    }
}
