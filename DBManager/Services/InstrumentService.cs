using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager.Services
{
    public static class InstrumentService
    {
        public static void AddCalibrationFiles(IEnumerable<CalibrationFiles> fileList)
        {
            // inserts a set of CalibrationFiles entries in the DB

            using (DBEntities entities = new DBEntities())
            {
                entities.CalibrationFiles.AddRange(fileList);
                entities.SaveChanges();
            }
        }


        public static IEnumerable<Instrument> GetCalibrationCalendar()
        {
            // Returns a list of the instruments under control, ordered by due calibration date

            using (DBEntities entities = new DBEntities())
            {
                return entities.Instruments.Include(inst => inst.CalibrationResponsible)
                                            .Include(inst => inst.InstrumentType)
                                            .Where(inst => inst.IsUnderControl)
                                            .OrderBy(inst => inst.CalibrationDueDate)
                                            .ToList();                                            
            }
        }

        public static IEnumerable<Instrument> GetInstruments()
        {
            // Returns all Instrument entities

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Instruments.Include(inst => inst.InstrumentType)
                                            .Include(inst => inst.Manufacturer)
                                            .Where(inst => true)
                                            .ToList();
            }
        }

        public static IEnumerable<InstrumentType> GetInstrumentTypes()
        {
            // Returns all InstrumentType entities

            using (var entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.InstrumentTypes
                                .OrderBy(insty => insty.Name)
                                .ToList();
            }
        }

        public static IEnumerable<MeasurableQuantity> GetMeasurableQuantities()
        {
            // Returns all Measurable Quantities

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.MeasurableQuantities.ToList();
            }
        }

        public static IEnumerable<MeasurementUnit> GetMeasurementUnits()
        {
            // Returns all measurement units

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.MeasurementUnits.ToList();
            }
        }
        
        public static int GetNextCalibrationNumber(int year)
        {
            // Returns the next available calibration number for a given year

            using (DBEntities entities = new DBEntities())
            {
                return entities.CalibrationReports
                                .Where(crep => crep.Year == year)
                                .Max(crep => crep.Number) + 1;                                
            }
        }
    }
}
