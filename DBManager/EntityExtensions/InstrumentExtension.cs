using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager
{
    public partial class Instrument
    {

        public IEnumerable<InstrumentMaintenanceEvent> GetMaintenanceEvents()
        {
            // Returns all InstrumentMaintenanceEvents for an Instrument
            
            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.InstrumentMaintenanceEvents.Include(ime => ime.Person)
                                                            .Where(ime => ime.InstrumentID == ID)
                                                            .ToList();
            }
        }
    }

    public static class InstrumentExtension
    {
        public static void AddFiles(this Instrument entry,
                                    IEnumerable<string> paths)
        {
            // Given a list of paths creates and adds a corresponding series of InstrumentFiles entities

            using (DBEntities entities = new DBEntities())
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

        public static void AddMethodAssociation(this Instrument entry,
                                                Method methodEntity)
        {
            // Creates a new Instrument/method association

            using (DBEntities entities = new DBEntities())
            {
                entities.Instruments.First(inst => inst.ID == entry.ID)
                                    .AssociatedMethods
                                    .Add(entities.Methods
                                    .First(mtd => mtd.ID == methodEntity.ID));

                entities.SaveChanges();
            }
        }

        public static void Create(this Instrument entry)
        {
            // Inserts a new Instrument entry

            using (DBEntities entities = new DBEntities())
            {
                entities.Instruments.Add(entry);

                entities.SaveChanges();
            }
        }

        public static void Delete(this Instrument entry)
        {
            // Deletes an Instrument entry

            using (DBEntities entities = new DBEntities())
            {
                entities.Entry(entities.Instruments
                        .First(ins => ins.ID == entry.ID))
                        .State = EntityState.Deleted;

                entities.SaveChanges();

                entry.ID = 0;
            }
        }

        public static IEnumerable<Method> GetAssociatedMethods(this Instrument entry)
        {
            // Returns all the methods not assigned to the instrument entry

            if (entry == null)
                return null;

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Methods.Include(mtd => mtd.Property)
                                        .Include(mtd => mtd.Standard.Organization)
                                        .Where(mtd => mtd.AssociatedInstruments
                                        .Any(instr => instr.ID == entry.ID))
                                        .ToList();
            }
        }

        public static DateTime? GetCalibrationDueDateFrom(this Instrument entry,
                                                        DateTime lastCalibration)
        {
            // Returns a Due date for the next calibration considering a given date and the calibration frequency
            
            return lastCalibration.Date.AddMonths(entry.CalibrationInterval);
        }

        public static IEnumerable<CalibrationReport> GetCalibrationReports(this Instrument entry)
        {
            // Returns all Calibration reports for an Instrument entry

            if (entry == null)
                return null;

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.CalibrationReports.Include(cal => cal.Laboratory)
                                                    .Include(cal => cal.Tech)
                                                    .Where(cal => cal.instrumentID == entry.ID)
                                                    .ToList();
            }
        }

        public static CalibrationReport GetLastCalibration(this Instrument entry)
        {
            //Returns the most recent calibration report for the instrument

            if (entry == null)
                return null;

            using (DBEntities entities = new DBEntities())
            {

                entities.Configuration.LazyLoadingEnabled = false;

                return entities.CalibrationReports
                                .Where(calrep => calrep.instrumentID == entry.ID)
                                .OrderByDescending(calrep => calrep.Date)
                                .FirstOrDefault();
            }
        }

        public static IEnumerable<InstrumentMeasurableProperty> GetMeasurableProperties(this Instrument entry)
        {
            // Returns all MeasurableProperties for an Instrument

            if (entry == null)
                return null;

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.InstrumentMeasurableProperties.Include(imp => imp.MeasurableQuantity)
                                                                .Include(imp => imp.UnitOfMeasurement)
                                                                .Where(imp => imp.InstrumentID == entry.ID)
                                                                .ToList();
            }
        }

        public static IEnumerable<Method> GetUnassociatedMethods(this Instrument entry)
        {
            // Returns all the methods not assigned to the instrument entry

            if (entry == null)
                return null;

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Methods.Include(mtd => mtd.Property)
                                        .Include(mtd => mtd.Standard.Organization)
                                        .Where(mtd => !mtd.AssociatedInstruments
                                        .Any(instr => instr.ID == entry.ID))
                                        .ToList();
            }
        }

        public static void Load(this Instrument entry)
        {
            // Loads the relevant Related entities into a given Instrument entry

            if (entry == null)
                return;

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                Instrument tempEntry = entities.Instruments.Include(inst => inst.AssociatedMethods
                                                            .Select(mtd => mtd.Standard.Organization))
                                                            .Include(inst => inst.AssociatedMethods
                                                            .Select(mtd => mtd.Property))
                                                            .Include(inst => inst.CalibrationReports
                                                            .Select(cal => cal.Laboratory))
                                                            .Include(inst => inst.InstrumentType)
                                                            .Include(inst => inst.Manufacturer)
                                                            .Include(inst => inst.Supplier)
                                                            .Include(inst => inst.Tests
                                                            .Select(tst => tst.Method.Property))
                                                            .Include(inst => inst.Tests
                                                            .Select(tst => tst.Method.Standard))
                                                            .First(inst => inst.ID == entry.ID);

                entry.AssociatedMethods = tempEntry.AssociatedMethods;
                entry.CalibrationReportAsReference = tempEntry.CalibrationReportAsReference;
                entry.CalibrationReports = tempEntry.CalibrationReports;
                entry.Code = tempEntry.Code;
                entry.Description = tempEntry.Description;
                entry.InstrumentType = tempEntry.InstrumentType;
                entry.InstrumentTypeID = tempEntry.InstrumentTypeID;
                entry.MaintenanceEvent = tempEntry.MaintenanceEvent;
                entry.Manufacturer = tempEntry.Manufacturer;
                entry.manufacturerID = tempEntry.manufacturerID;
                entry.Model = tempEntry.Model;
                entry.SerialNumber = tempEntry.SerialNumber;
                entry.Supplier = tempEntry.Supplier;
                entry.supplierID = tempEntry.supplierID;
                entry.Tests = tempEntry.Tests;
            }
        }

        public static void RemoveMethodAssociation(this Instrument entry,
                                                    Method methodEntity)
        {
            // Creates a new Instrument/method association

            using (DBEntities entities = new DBEntities())
            {
                Instrument tempInstrument = entities.Instruments.First(inst => inst.ID == entry.ID);
                Method tempMethod = tempInstrument.AssociatedMethods.First(mtd => mtd.ID == methodEntity.ID);

                tempInstrument.AssociatedMethods.Remove(tempMethod);

                entities.SaveChanges();
            }
        }

        public static void Update(this Instrument entry)
        {
            // Updates a given Instrument entry

            using (DBEntities entities = new DBEntities())
            {
                entities.Instruments.AddOrUpdate(entry);

                entities.SaveChanges();
            }
        }

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
    }
}
