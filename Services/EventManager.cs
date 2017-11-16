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

            _eventAggregator.GetEvent<ExternalReportCreationRequested>()
                            .Subscribe(
                () =>
                {
                    ExternalReport tmp = ServiceProvider.StartExternalReportCreation();
                    if (tmp != null)
                    {
                        _eventAggregator.GetEvent<ExternalReportCreated>()
                                        .Publish(tmp);
                        _eventAggregator.GetEvent<StatusNotificationIssued>()
                                        .Publish("Report esterno creato con il numero " + tmp.InternalNumber);
                    }

                });

            _eventAggregator.GetEvent<InstrumentTypeCreationRequested>()
                            .Subscribe(
                            () =>
                            {
                                InstrumentType output = ServiceProvider.CreateNewInstrumentType();
                                if (output != null)
                                    _eventAggregator.GetEvent<InstrumentTypeCreated>()
                                                    .Publish(output);
                            });

            _eventAggregator.GetEvent<MaintenanceEventCreationRequested>()
                            .Subscribe(instrumentEntry =>
                            {
                                Views.MaintenanceEventCreationDialog newEventDialog = new Views.MaintenanceEventCreationDialog();
                                newEventDialog.InstrumentInstance = instrumentEntry;

                                if (newEventDialog.ShowDialog() == true)
                                {
                                    _eventAggregator.GetEvent<MaintenanceEventCreated>()
                                                    .Publish(newEventDialog.InstrumentEventInstance);
                                }
                            });

            _eventAggregator.GetEvent<MeasurableQuantityCreationRequested>()
                            .Subscribe(
                            () =>
                            {
                                MeasurableQuantity output = ServiceProvider.CreateNewMeasurableQuantity();
                                if (output != null)
                                    _eventAggregator.GetEvent<MeasurableQuantityCreated>()
                                                    .Publish(output);
                            });

            _eventAggregator.GetEvent<NewCalibrationRequested>()
                            .Subscribe(
                            instrument =>
                            {
                                CalibrationReport tempReport = ServiceProvider.RegisterNewCalibration(instrument);

                                if (tempReport != null)
                                {
                                    _eventAggregator.GetEvent<CalibrationIssued>()
                                                    .Publish(tempReport);
                                    _eventAggregator.GetEvent<StatusNotificationIssued>()
                                                    .Publish("Report taratura creato con il numero " + tempReport.GetFormattedNumber());
                                }
                            });

            _eventAggregator.GetEvent<OrganizationCreationRequested>()
                        .Subscribe(() =>
                        {
                            Organization tempOrg = ServiceProvider.CreateNewOrganization();
                            if (tempOrg != null)
                            {
                                EntityChangedToken entityChangedToken = new EntityChangedToken(tempOrg,
                                                                                                EntityChangedToken.EntityChangedAction.Created);

                                _eventAggregator.GetEvent<OrganizationChanged>()
                                                .Publish(entityChangedToken);
                            }
                        });

            _eventAggregator.GetEvent<OrganizationRoleCreationRequested>()
                        .Subscribe(() =>
                        {
                            ServiceProvider.CreateNewOrganizationRole();
                        });

            _eventAggregator.GetEvent<PersonCreationRequested>()
                            .Subscribe(
                            () =>
                            {
                                Person tempPerson = ServiceProvider.AddPerson();

                                if (tempPerson != null)
                                {
                                    EntityChangedToken entityChangedToken = new EntityChangedToken(tempPerson,
                                                                                                    EntityChangedToken.EntityChangedAction.Created);

                                    _eventAggregator.GetEvent<PersonChanged>()
                                                    .Publish(entityChangedToken);
                                }
                            });

            _eventAggregator.GetEvent<ProjectCostUpdateRequested>()
                            .Subscribe(
                            () =>
                            {
                                ServiceProvider.UpdateProjectCosts();
                            });

            _eventAggregator.GetEvent<UserRoleCreationRequested>()
                            .Subscribe(
                            () =>
                            {
                                Controls.Views.StringInputDialog stringDialog = new Controls.Views.StringInputDialog();
                                if (stringDialog.ShowDialog() == true)
                                    ServiceProvider.AddUserRole(stringDialog.InputString);
                            });

            _eventAggregator.GetEvent<ReportCreated>().Subscribe(
                report =>
                {
                    Batch targetBatch = MaterialService.GetBatch(report.BatchID);

                    if (targetBatch.BasicReportID == null)
                    {
                        targetBatch.BasicReportID = report.ID;
                        targetBatch.Update();
                    }
                });

            _eventAggregator.GetEvent<ReportCreationRequested>().Subscribe(
                token =>
                {
                    Views.ReportCreationDialog creationDialog = new Views.ReportCreationDialog();

                    if (token.TargetBatch != null)
                        creationDialog.Batch = token.TargetBatch;

                    if (creationDialog.ShowDialog() == true)
                    {
                        _eventAggregator.GetEvent<ReportCreated>()
                                        .Publish(creationDialog.ReportInstance);
                    }
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

                        _eventAggregator.GetEvent<ReportCompleted>()
                                        .Publish(report);

                        if (report.ParentTask != null)
                            _eventAggregator.GetEvent<TaskStatusCheckRequested>()
                                            .Publish(report.ParentTask);
                    }

                });
        }
    }
}
