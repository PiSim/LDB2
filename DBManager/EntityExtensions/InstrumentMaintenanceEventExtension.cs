using DBManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager.EntityExtensions
{
    public static class InstrumentMaintenanceEventExtension
    {
        public static void Create(this InstrumentMaintenanceEvent entry)
        {
            //Inserts a new InstrumentMaintenanceEvent entry in the DB

            using (DBEntities entities = new DBEntities())
            {
                entities.InstrumentMaintenanceEvents.Add(entry);

                entities.SaveChanges();
            }
        }
    }
}
