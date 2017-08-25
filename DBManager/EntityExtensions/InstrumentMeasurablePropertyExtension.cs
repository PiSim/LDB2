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
        public static DateTime GetCalibrationDueDateFrom(this InstrumentMeasurableProperty entry,
                                                        DateTime lastCalibration)
        {
            // Returns a Due date for the next calibration considering a given date and the property's calibration frequency

            return lastCalibration.Date.AddMonths(entry.ControlPeriod);
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

        public static void Update(this InstrumentMeasurableProperty entry)
        {
            // Updates an InstrumentMeasurableProperty entry

            using (DBEntities entities = new DBEntities())
            {
                entities.InstrumentMeasurableProperties.AddOrUpdate(entry);

                entities.SaveChanges();
            }
        }

        public static bool UpdateCalibrationDueDate(this InstrumentMeasurableProperty entry)
        {
            // Updates the value for CalibrationDueDate using the latest calibration in the DB and the parameters set in the entry instance
            // Returns true if the new value differs from the old one

            DateTime oldvalue = entry.CalibrationDue.Value;

            if (!entry.IsUnderControl)
                entry.CalibrationDue = null;

            else 
            {
                DateTime lastCalibration = entry.GetLastCalibration().Date;

                if (lastCalibration == null)
                    entry.CalibrationDue = DateTime.Now.Date;

                else
                    entry.CalibrationDue = entry.GetCalibrationDueDateFrom(lastCalibration);
            }

            return entry.CalibrationDue != oldvalue;

        }
    }
}
