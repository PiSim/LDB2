using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager.EntityExtensions
{
    public static class ReportFileExtension
    {
        public static void Create(this ReportFile entry)
        {
            // Inserts a ReportFile entry in the DB

            using (DBEntities entities = new DBEntities())
            {
                entities.ReportFiles.Add(entry);
                entities.SaveChanges();
            }
        }

        public static void Delete(this ReportFile entry)
        {
            // Deletes a ReportFile entry

            using (DBEntities entities = new DBEntities())
            {
                entities.Entry(entities.ReportFiles
                        .First(repf => repf.ID == entry.ID))
                        .State = EntityState.Deleted;

                entities.SaveChanges();

                entry.ID = 0;
            }
        }
    }
}
