using Controls.Views;
using DBManager;
using Infrastructure;
using Infrastructure.Events;
using Infrastructure.Wrappers;
using Microsoft.Practices.Unity;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Reports
{
    public class ReportService : IReportService
    {
        private EventAggregator _eventAggregator;
        private IUnityContainer _container;

        public ReportService(EventAggregator aggregator,
                            IUnityContainer container)
        {
            _container = container;
            _eventAggregator = aggregator;
        }

        [Obsolete]
        public static PurchaseOrder AddPOToExternalReport(ExternalReport target)
        {

            using (DBEntities entities = new DBEntities())
            {
                ExternalReport targetReport = entities.ExternalReports.First(xtr => xtr.ID == target.ID);

                NewPODialog poDialog = new NewPODialog();
                poDialog.SetSupplier(targetReport.ExternalLab);

                if (poDialog.ShowDialog() == true)
                {
                    PurchaseOrder output = entities.PurchaseOrders.FirstOrDefault(po => po.Number == poDialog.Number);

                    if (output == null)
                    {
                        output = new PurchaseOrder();
                        output.Number = poDialog.Number;
                        output.Organization = entities.Organizations
                            .First(sup => sup.ID == poDialog.Supplier.ID);
                        output.Total = poDialog.Total;
                    }

                    targetReport.PO = output;
                    entities.SaveChanges();

                    return output;
                }

                else
                    return null;
            }
        }

        // Interface members

        ExternalReport IReportService.CreateExternalReport() => CreateExternalReport();

        Report IReportService.CreateReport() => CreateReport();

        Report IReportService.CreateReport(Task parentTask) => CreateReport(parentTask);


        ExternalReport CreateExternalReport()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Starts the process of creating a new Report instance via the ReportCreationDialog
        /// If a Task instance is given it will be used as template
        /// If creation is successful an event is raised and the new Report returned
        /// </summary>
        /// <param name="parentTask">A Task entry to use as template</param>
        /// <returns>The newly created report instance</returns>
        Report CreateReport(Task parentTask = null)
        {
            Views.ReportCreationDialog creationDialog = new Views.ReportCreationDialog();

            // If task is given sets the instance in the ReportCreationDialog
            if (parentTask != null)
                creationDialog.TaskInstance = parentTask;

            // If creation is succesful raise event and return the instance, otherwise return null
            if (creationDialog.ShowDialog() == true)
            {
                _eventAggregator.GetEvent<ReportCreated>()
                                .Publish(creationDialog.ReportInstance);
                return creationDialog.ReportInstance;
            }

            return null;
        }
    }
}
