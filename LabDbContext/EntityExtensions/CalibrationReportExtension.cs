using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace LabDbContext.EntityExtensions
{
    public static class CalibrationReportExtension
    {
        #region Methods

        public static void AddReference(this CalibrationReport entry,
                                        Instrument referenceEntry)
        {
            // Adds an association with the given reference instrument

            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.CalibrationReports
                        .First(calrep => calrep.ID == entry.ID)
                        .ReferenceInstruments
                        .Add(entities
                        .Instruments
                        .First(inst => inst.ID == referenceEntry.ID));

                entities.SaveChanges();
            }
        }
        public static IEnumerable<CalibrationFiles> GetFiles(this CalibrationReport entry)
        {
            // returns all CAlibrationfiles associated with a given CalibrationReport Entry

            if (entry == null)
                return new List<CalibrationFiles>();

            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.CalibrationFiles
                                .Where(calf => calf.ReportID == entry.ID)
                                .ToList();
            }
        }


        public static IEnumerable<CalibrationReportInstrumentPropertyMapping> GetPropertyMappings(this CalibrationReport entry)
        {
            // Returns all associated CalibrationReportPropertyMapping entities

            if (entry == null)
                return null;

            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.CalibrationReportInstrumentPropertyMappings
                                .Include(cripm => cripm.InstrumentMeasurableProperty.MeasurableQuantity)
                                .Include(cripm => cripm.InstrumentMeasurableProperty.UnitOfMeasurement)
                                .Where(cripm => cripm.CalibrationReportID == entry.ID)
                                .ToList();
            }
        }

        public static IEnumerable<Instrument> GetReferenceInstruments(this CalibrationReport entry)
        {
            // Returns all Reference instruments for a given CalibrationReport entry

            if (entry == null)
                return new List<Instrument>();

            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.CalibrationReports
                                .Include(calr => calr.ReferenceInstruments
                                .Select(refin => refin.InstrumentType))
                                .Include(calr => calr.ReferenceInstruments
                                .Select(refin => refin.Manufacturer))
                                .First(calr => calr.ID == entry.ID)
                                .ReferenceInstruments
                                .ToList();
            }
        }
        public static void RemoveReference(this CalibrationReport entry,
                                            Instrument referenceEntry)
        {
            // Deletes an association between a CalibrationReport and a reference instrument

            using (LabDbEntities entities = new LabDbEntities())
            {
                CalibrationReport tempEntry = entities.CalibrationReports
                                                        .First(calrep => calrep.ID == entry.ID);

                tempEntry.ReferenceInstruments
                        .Remove(tempEntry.ReferenceInstruments
                        .First(inst => inst.ID == referenceEntry.ID));

                entities.SaveChanges();
            }
        }

        #endregion Methods
    }
}