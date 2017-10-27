using DBManager;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager.EntityExtensions
{
    public static class InstrumentMeasurablePropertyExtension
    {
        public static void Create(this InstrumentMeasurableProperty entry)
        {
            // Inserts a new InstrumentMeasurableProperty entry in the DB

            using (DBEntities entities = new DBEntities())
            {
                entities.InstrumentMeasurableProperties.Add(entry);
                entities.SaveChanges();
            }
        }

        public static CalibrationReport GetLastCalibration(this InstrumentMeasurableProperty entry)
        {
            // Returns the most recent CalibrationReport for this entry, or null if none exist

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.CalibrationReports
                                .Where(calrep => calrep
                                .InstrumentMeasurablePropertyMappings
                                .Any(impm => impm.MeasurablePropertyID == entry.ID))
                                .OrderByDescending(calrep => calrep.Date)
                                .FirstOrDefault();
            }
        }

        public static IEnumerable<MeasurementUnit> GetMeasurementUnits(this InstrumentMeasurableProperty entry)
        {
            // Returns a list of possible UM for a given property

            if (entry == null)
                return new List<MeasurementUnit>();

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.MeasurementUnits
                                .Where(um => entities
                                .InstrumentMeasurableProperties
                                .FirstOrDefault(imp => imp.ID == entry.ID)
                                .MeasurableQuantityID == um.MeasurableQuantityID)
                                .ToList();
            }
        }

        public static void Update(this InstrumentMeasurableProperty entry)
        {
            // Updates an InstrumentMeasurableProperty entry

            using (DBEntities entities = new DBEntities())
            {
                entities.InstrumentMeasurableProperties.AddOrUpdate(entry);

                entities.SaveChanges();
            }
        }
    }
}
