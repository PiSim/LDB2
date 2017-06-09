using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager.EntityExtensions
{
    public static class ExternalReportExtension
    {
        public static void AddBatch(this ExternalReport entry,
                                    Batch batchEntity)
        {
            // Adds a Batch to an ExternalReport entry

            using (DBEntities entities = new DBEntities())
            {
                entities.ExternalReports.First(ext => ext.ID == entry.ID)
                                        .Batches
                                        .Add(entities.Batches
                                        .First(btc => btc.ID == batchEntity.ID));

                entities.SaveChanges();
            }
        }

        public static IEnumerable<ExternalReportFile> GetExternalReportFiles(this ExternalReport entry)
        {
            // Returns all the files for an external report entry

            if (entry == null)
                return null;

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.ExternalReportFiles.Where(extf => extf.ExternalReportID == entry.ID)
                                .ToList();
            }
        }
    }
}
