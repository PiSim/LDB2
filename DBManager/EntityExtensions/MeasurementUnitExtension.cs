using DBManager;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager.EntityExtensions
{
    public static class MeasurementUnitExtension
    {
        public static void Create(this MeasurementUnit entry)
        {
            // Inserts a MeasurementUnit entry in the DB

            using (DBEntities entities = new DBEntities())
            {
                entities.MeasurementUnits.Add(entry);

                entities.SaveChanges();
            }
        }
    }
}
