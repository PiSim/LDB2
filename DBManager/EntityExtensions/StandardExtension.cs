using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager.EntityExtensions
{
    public static class StandardExtension
    {
        public static void Create(this Std entry)
        {
            // Inserts a new Std entry in the DB

            using (DBEntities entities = new DBEntities())
            {
                entities.Stds.Add(entry);
                entities.SaveChanges();
            }
        }

        public static void Delete(this Std entry)
        {
            // Deletes an Std entry

            using (DBEntities entities = new DBEntities())
            {
                Std tempEntry = entities.Stds.First(std => std.ID == entry.ID);

                entities.Entry(tempEntry).State = System.Data.Entity.EntityState.Deleted;

                entities.SaveChanges();
            }
        }
    }
}
