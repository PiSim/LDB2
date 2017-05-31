using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager.EntityExtensions
{
    public static class ExternalReportFileExtension
    {
        public static void Create(this ExternalReportFile entry)
        {
            // Inserts an ExternalReportFile entry

            using (DBEntities entities = new DBEntities())
            {
                entities.ExternalReportFiles.Add(entry);
                entities.SaveChanges();
            }
        }

        public static void Delete(this ExternalReportFile entry)
        {
            // Deletes an ExternalReportFile entry

            using (DBEntities entities = new DBEntities())
            {
                entities.Entry(entities.ExternalReportFiles
                        .First(exrf => exrf.ID == entry.ID))
                        .State = EntityState.Deleted;

                entities.SaveChanges();
            }
        }
    }
}
