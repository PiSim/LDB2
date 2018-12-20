using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace LabDbContext
{
    public static class InstrumentExtension
    {
        #region Methods

        [Obsolete]
        public static void AddFiles(this Instrument entry,
                                    IEnumerable<string> paths)
        {
            // Given a list of paths creates and adds a corresponding series of InstrumentFiles entities

            using (LabDbEntities entities = new LabDbEntities())
            {
                foreach (string pth in paths)
                {
                    InstrumentFiles inf = new InstrumentFiles()
                    {
                        Path = pth,
                        InstrumentID = entry.ID
                    };

                    entities.InstrumentFiles.Add(inf);
                }

                entities.SaveChanges();
            }
        }

        [Obsolete]
        public static DateTime? GetCalibrationDueDateFrom(this Instrument entry,
                                                        DateTime lastCalibration)
        {
            // Returns a Due date for the next calibration considering a given date and the calibration frequency

            return lastCalibration.Date.AddMonths(entry.CalibrationInterval);
        }

        [Obsolete]
        public static CalibrationReport GetLastCalibration(this Instrument entry)
        {
            //Returns the most recent calibration report for the instrument

            if (entry == null)
                return null;

            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.CalibrationReports
                                .Where(calrep => calrep.instrumentID == entry.ID)
                                .OrderByDescending(calrep => calrep.Date)
                                .FirstOrDefault();
            }
        }

        #endregion Methods
    }

    public partial class Instrument
    {
        #region Methods

        [Obsolete]
        public IEnumerable<InstrumentMaintenanceEvent> GetMaintenanceEvents()
        {
            // Returns all InstrumentMaintenanceEvents for an Instrument

            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.InstrumentMaintenanceEvents.Include(ime => ime.Person)
                                                            .Where(ime => ime.InstrumentID == ID)
                                                            .ToList();
            }
        }

        #endregion Methods
    }
}