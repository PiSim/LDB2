using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager.EntityExtensions
{
    public static class CalibrationReportExtension
    {
        public static void Create(this CalibrationReport entry)
        {
            // Inserts a calibration entry in the DB

            using (DBEntities entities = new DBEntities())
            {
                entities.CalibrationReports.Add(entry);
                entities.SaveChanges();
            }
        }

        public static void Delete(this CalibrationReport entry)
        {
            // Deletes a Calibration entry from the DB

            using (DBEntities entities = new DBEntities())
            {
                entities.Entry(entities
                        .CalibrationReports
                        .First(crep => crep.ID == entry.ID))
                        .State = System.Data.Entity.EntityState.Deleted;

                entities.SaveChanges();
                entry.ID = 0;
            }
        }

        public static void Update(this CalibrationReport entry)
        {
            // Updates a CAlibrationReport entry

            using (DBEntities entities = new DBEntities())
            {
                entities.CalibrationReports
                        .AddOrUpdate(entry);

                entities.SaveChanges();
            }
        }
    }
}
