using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager.EntityExtensions
{
    public static class InstrumentExtension
    {
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

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;
                
                Instrument tempEntry = entities.Instruments.Include(inst => inst.AssociatedMethods
                                                            .Select(mtd => mtd.Standard.Organization))
                                                            .Include(inst => inst.AssociatedMethods
                                                            .Select(mtd => mtd.Property))
                                                            .Include(inst => inst.CalibrationReports
                                                            .Select(cal => cal.Laboratory))
                                                            .Include(inst => inst.CalibrationResponsible)
                                                            .Include(inst => inst.InstrumentType)
                                                            .Include(inst => inst.MaintenanceEvent
                                                            .Select(mte => mte.Organization))
                                                            .Include(inst => inst.Manufacturer)
                                                            .Include(inst => inst.Supplier)
                                                            .Include(inst => inst.Tests
                                                            .Select(tst => tst.Method.Property))
                                                            .Include(inst => inst.Tests
                                                            .Select(tst => tst.Method.Standard.CurrentIssue))
                                                            .First(inst => inst.ID == entry.ID);

                entry.AssociatedMethods = tempEntry.AssociatedMethods;
                entry.CalibrationDueDate = tempEntry.CalibrationDueDate;
                entry.CalibrationReportAsReference = tempEntry.CalibrationReportAsReference;
                entry.CalibrationReports = tempEntry.CalibrationReports;
                entry.CalibrationResponsible = tempEntry.CalibrationResponsible;
                entry.calibration_responsibleID = tempEntry.calibration_responsibleID;
                entry.Code = tempEntry.Code;
                entry.ControlPeriod = tempEntry.ControlPeriod;
                entry.Description = tempEntry.Description;
                entry.InstrumentType = tempEntry.InstrumentType;
                entry.InstrumentTypeID = tempEntry.InstrumentTypeID;
                entry.IsUnderControl = tempEntry.IsUnderControl;
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
                Instrument tempEntry = entities.Instruments.First(inst => inst.ID == entry.ID);

                entities.Entry(tempEntry).CurrentValues.SetValues(entry);

                entities.SaveChanges();
            }
        }
    }
}
