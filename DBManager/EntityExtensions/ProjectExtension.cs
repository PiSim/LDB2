using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager
{
    public partial class Project
    {
        public void UpdateExternalReportCost()
        {
            using (DBEntities entities = new DBEntities())
            {
                IEnumerable<PurchaseOrder> _poList = entities.PurchaseOrders
                                                    .Where(po => po.ExternalReports
                                                    .Any(exr => exr.ProjectID == this.ID));

                TotalExternalCost = (_poList.Count() == 0) ? 0 : _poList.Sum(po => po.Total);

            }
        }
    }

    public static class ProjectExtension
    {
        public static IEnumerable<Material> GetMaterials(this Project entry)
        {
            // Returns all Material entities for a Project

            if (entry == null)
                return null;

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Materials.Include(mat => mat.Aspect)
                                        .Include(mat => mat.ExternalConstruction)
                                        .Include(mat => mat.MaterialLine)
                                        .Include(mat => mat.MaterialType)
                                        .Include(mat => mat.Recipe.Colour)
                                        .Where(con => con.ProjectID == entry.ID)
                                        .ToList();
            }
        }

        public static IEnumerable<ExternalReport> GetExternalReports(this Project entry)
        {
            // Returns the external reports for a Project

            using (DBEntities entities = new DBEntities())
            {
                return entities.ExternalReports
                                .Where(erep => erep
                                .ProjectID == entry.ID);
            }
        }

        public static IEnumerable<PurchaseOrder> GetPurchaseOrders(this Project entry)
        {

            // Returns all Purchase orders for a project

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.PurchaseOrders
                                .Where(po => po
                                .ExternalReports
                                .Any(exr => exr
                                .ProjectID == entry.ID))
                                .ToList();
            }
        }

        public static IEnumerable<Test> GetTests(this Project entry)
        {
            // Returns all the tests for a project

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Tests
                                .Where(tst => tst
                                .Report
                                .Batch
                                .Material
                                .ProjectID == entry.ID)
                                .ToList();
            }
        }
    }

}
