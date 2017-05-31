using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager.EntityExtensions
{
    public static class StandardFileExtension
    {
        public static void Create(this StandardFile entry)
        {
            // Inserts a StandardFile entry in the DB

            using (DBEntities entities = new DBEntities())
            {
                entities.StandardFiles.Add(entry);
                entities.SaveChanges();
            }
        }

        public static void Delete(this StandardFile entry)
        {
            // Deletes a standardFile entry

            using (DBEntities entities = new DBEntities())
            {
                entities.Entry(entities.StandardFiles
                        .First(stdf => stdf.ID == entry.ID))
                        .State = EntityState.Deleted;

                entities.SaveChanges();
            }
        }
    }
}
