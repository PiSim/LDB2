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

        public static IEnumerable<CalibrationFiles> GetFiles(this CalibrationReport entry)
        {
            // returns all CAlibrationfiles associated with a given CalibrationReport Entry

            if (entry == null)
                return new List<CalibrationFiles>();

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.CalibrationFiles
                                .Where(calf => calf.ReportID == entry.ID)
                                .ToList();
            }
        }

        public static string GetFormattedNumber(this CalibrationReport entry)
        {
            // Returns a string uniquely representing a Calibration report 
            // the string is generated from the report's number and year properties

            return entry.Year.ToString() + entry.Number.ToString("D4"); 
        }

        public static IEnumerable<CalibrationReportInstrumentPropertyMapping> GetPropertyMappings(this CalibrationReport entry)
        {
            // Returns all associated CalibrationReportPropertyMapping entities

            if (entry == null)
                return null;

            using (DBEntities entities = new DBEntities())
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

            using (DBEntities entities = new DBEntities())
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
