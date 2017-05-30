using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager.EntityExtensions
{
    public static class SubMethodExtension
    {

        public static void Delete(this SubMethod entry)
        {
            // Deletes a SubMethod entry from the DB

            using (DBEntities entities = new DBEntities())
            {
                entities.Entry(entities.SubMethods.First(smtd => smtd.ID == entry.ID))
                        .State = System.Data.Entity.EntityState.Deleted;

                entities.SaveChanges();
                entry.ID = 0;
            }
        }
    }
}
