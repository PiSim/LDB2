using DBManager.Services;
using Infrastructure.Events;
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

            _eventAggregator.GetEvent<ReportCreationRequested>().Subscribe(
                token =>
                {
                    Views.ReportCreationDialog creationDialog = new Views.ReportCreationDialog();

                    if (token.TargetBatch != null)
                        creationDialog.Batch = token.TargetBatch;

                    if (creationDialog.ShowDialog() == true)
                        _eventAggregator.GetEvent<ReportListUpdateRequested>().Publish();
                });
        }
    }
}
