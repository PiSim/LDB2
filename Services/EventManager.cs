using DBManager.Services;
using DBManager.EntityExtensions;
using Infrastructure.Events;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBManager;

namespace Services
{
    internal class EventManager
    {
        private EventAggregator _eventAggregator;

        public EventManager(EventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;

            _eventAggregator.GetEvent<BatchCreationRequested>()
                            .Subscribe(
                            () =>
                            {
                                Batch tmp = ServiceProvider.CreateBatch();
                                if (tmp != null)
                                    _eventAggregator.GetEvent<BatchCreated>()
                                                    .Publish(tmp);
                            });

            _eventAggregator.GetEvent<MaintenanceEventCreationRequested>()
                            .Subscribe(instrumentEntry =>
                            {
                                Views.MaintenanceEventCreationDialog newEventDialog = new Views.MaintenanceEventCreationDialog();
                                if (newEventDialog.ShowDialog() == true)
                                {

                                }
                            });
            
            _eventAggregator.GetEvent<NewCalibrationRequested>()
                            .Subscribe(
                            instrument =>
                            {
                                CalibrationReport tempReport = ServiceProvider.RegisterNewCalibration(instrument);

                                if (tempReport != null)
                                    _eventAggregator.GetEvent<CalibrationIssued>()
                                                    .Publish(tempReport);
                            });

            _eventAggregator.GetEvent<UserRoleCreationRequested>()
                            .Subscribe(
                            () =>
                            {
                                Controls.Views.StringInputDialog stringDialog = new Controls.Views.StringInputDialog();
                                if (stringDialog.ShowDialog() == true)
                                    ServiceProvider.AddUserRole(stringDialog.InputString);
                            });

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

                    bool areTestsComplete = report.AreTestsComplete();

                    if ((report.IsComplete && !areTestsComplete)
                        || (!report.IsComplete && areTestsComplete))
                    {
                        report.IsComplete = !report.IsComplete;
                        report.Update();

                        _eventAggregator.GetEvent<ReportListUpdateRequested>()
                                        .Publish();

                        if (report.ParentTask != null)
                            _eventAggregator.GetEvent<TaskStatusCheckRequested>()
                                            .Publish(report.ParentTask);
                    }

                });
        }
    }
}
