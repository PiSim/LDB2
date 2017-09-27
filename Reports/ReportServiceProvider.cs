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
using System.Text;
using System.Threading.Tasks;

namespace Reports
{
    public class ReportServiceProvider
    {
        private DBEntities _entities;
        private EventAggregator _eventAggregator;
        private IUnityContainer _container;

        public ReportServiceProvider(DBEntities entities,
                                    EventAggregator aggregator,
                                    IUnityContainer container)
        {
            _container = container;
            _entities = entities;
            _eventAggregator = aggregator;
        }

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
                        output.Currency = entities.Currencies
                             .First(crn => crn.ID == poDialog.Currency.ID);
                        output.Number = poDialog.Number;
                        output.OrderDate = poDialog.Date;
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

        
    }
}
