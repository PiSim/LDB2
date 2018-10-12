using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
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
        public static void AddMethodAssociation(this Instrument entry,
                                                Method methodEntity)
        {
            // Creates a new Instrument/method association

            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.Instruments.First(inst => inst.ID == entry.ID)
                                    .AssociatedMethods
                                    .Add(entities.Methods
                                    .First(mtd => mtd.ID == methodEntity.ID));

                entities.SaveChanges();
            }
        }
        [Obsolete]
        public static IEnumerable<Method> GetAssociatedMethods(this Instrument entry)
        {
            // Returns all the methods not assigned to the instrument entry

            if (entry == null)
                return null;

            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Methods.Include(mtd => mtd.Property)
                                        .Include(mtd => mtd.Standard.Organization)
                                        .Where(mtd => mtd.AssociatedInstruments
                                        .Any(instr => instr.ID == entry.ID))
                                        .ToList();
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
        [Obsolete]
        public static IEnumerable<InstrumentMeasurableProperty> GetMeasurableProperties(this Instrument entry)
        {
            // Returns all MeasurableProperties for an Instrument

            if (entry == null)
                return null;

            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.InstrumentMeasurableProperties.Include(imp => imp.MeasurableQuantity)
                                                                .Include(imp => imp.UnitOfMeasurement)
                                                                .Where(imp => imp.InstrumentID == entry.ID)
                                                                .ToList();
            }
        }
        [Obsolete]
        public static IEnumerable<Method> GetUnassociatedMethods(this Instrument entry)
        {
            // Returns all the methods not assigned to the instrument entry

            if (entry == null)
                return null;

            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Methods.Include(mtd => mtd.Property)
                                        .Include(mtd => mtd.Standard.Organization)
                                        .Where(mtd => !mtd.AssociatedInstruments
                                        .Any(instr => instr.ID == entry.ID))
                                        .ToList();
            }
        }
        [Obsolete]
        public static void RemoveMethodAssociation(this Instrument entry,
                                                    Method methodEntity)
        {
            // Creates a new Instrument/method association

            using (LabDbEntities entities = new LabDbEntities())
            {
                Instrument tempInstrument = entities.Instruments.First(inst => inst.ID == entry.ID);
                Method tempMethod = tempInstrument.AssociatedMethods.First(mtd => mtd.ID == methodEntity.ID);

                tempInstrument.AssociatedMethods.Remove(tempMethod);

                entities.SaveChanges();
            }
        }
        [Obsolete]
        public static bool UpdateCalibrationDueDate(this Instrument entry)
        {
            // Updates the value for CalibrationDueDate using the latest calibration in the DB and the parameters set in the entry instance
            // Returns true if the new value differs from the old one

            DateTime? oldvalue = (entry.CalibrationDueDate == null) ? (DateTime?)null : entry.CalibrationDueDate.Value;

            if (!entry.IsUnderControl)
                entry.CalibrationDueDate = null;
            else
            {
                CalibrationReport lastCalibration = entry.GetLastCalibration();

                if (lastCalibration == null || lastCalibration.Date == null)
                    entry.CalibrationDueDate = DateTime.Now.Date;
                else
                    entry.CalibrationDueDate = entry.GetCalibrationDueDateFrom(lastCalibration.Date);
            }

            return entry.CalibrationDueDate != oldvalue;
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