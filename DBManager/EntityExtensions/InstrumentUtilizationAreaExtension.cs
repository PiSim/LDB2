using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager.EntityExtensions
{
    public static class InstrumentUtilizationAreaExtension
    {
        public static void Create(this InstrumentUtilizationArea entry)
        {
            // Inserts a new InstrumentUtilizationArea entry in the DB

            using (DBEntities entities = new DBEntities())
            {
                entities.InstrumentUtilizationAreas.Add(entry);
                entities.SaveChanges();
            }
        }


        public static void Delete(this InstrumentUtilizationArea entry)
        {
            // Deletes an InstrumentUtilizationArea entry

            using (DBEntities entities = new DBEntities())
            {
                entities.Entry(entities
                        .InstrumentUtilizationAreas
                        .First(iua => iua.ID == entry.ID))
                        .State = EntityState.Deleted;

                entities.SaveChanges();
                entry.ID = 0;
            }
        }
    }
}
