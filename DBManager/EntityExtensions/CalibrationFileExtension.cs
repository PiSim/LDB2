using DBManager;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager.EntityExtensions
{
    public static class CalibrationFileExtension
    {
        public static void Delete(this CalibrationFiles entry)
        {
            // DEletes a CalibrationFile entry

            using (DBEntities entities = new DBEntities())
            {
                entities.Entry(entities
                        .CalibrationFiles
                        .First(calf => calf.ID == entry.ID))
                        .State = System.Data.Entity.EntityState.Deleted;

                entities.SaveChanges();
                entry.ID = 0;
            }
        }
    }
}
