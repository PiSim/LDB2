using DBManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Events
{
    public static class MeasurableQuantityExtension
    {
        public static void Delete(this MeasurableQuantity entry)
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Entry(entities
                        .MeasurableQuantities
                        .First(meq => meq.ID == entry.ID))
                        .State = System.Data.Entity.EntityState.Deleted;

                entities.SaveChanges();

                entry.ID = 0;
            }
        }
    }
}
