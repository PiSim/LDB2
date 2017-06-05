using DBManager.Services;
using DBManager.EntityExtensions;
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

            _eventAggregator.GetEvent<ReportStatusCheckRequested>()
                            .Subscribe(
                report =>
                {
                    // areTestsComplete is true if all tests are marked as complete

                    bool areTestsComplete = report.Tests.Any(tst => !tst.IsComplete);

                    if ((report.IsComplete && !areTestsComplete)
                        || (!report.IsComplete && areTestsComplete))
                    {
                        report.IsComplete = !report.IsComplete;
                        report.Update();

                        if (report.ParentTask != null)
                            _eventAggregator.GetEvent<TaskStatusCheckRequested>()
                                            .Publish(report.ParentTask);
                    }

                });
        }
    }
}
