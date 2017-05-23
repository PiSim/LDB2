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
        #region Operations for Instrument entities

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

        public static void Load(this Instrument entry)
        {
            // Loads the relevant Related entities into a given Instrument entry

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                entities.Instruments.Attach(entry);

                Instrument tempEntry = entities.Instruments.Include(inst => inst.AssociatedMethods
                                                            .Select(mtd => mtd.Standard.Organization))
                                                            .Include(inst => inst.AssociatedMethods
                                                            .Select(mtd => mtd.Property))
                                                            .Include(inst => inst.CalibrationResponsible)
                                                            .Include(inst => inst.MaintenanceEvent
                                                            .Select(mte => mte.Organization))
                                                            .Include(inst => inst.Manufacturer)
                                                            .Include(inst => inst.Supplier)
                                                            .Include(inst => inst.Tests
                                                            .Select(tst => tst.Method.Property))
                                                            .Include(inst => inst.Tests
                                                            .Select(tst => tst.Method.Standard.CurrentIssue))
                                                            .First(inst => inst.ID == entry.ID);

                entities.Entry(entry).CurrentValues.SetValues(entry);
            }
        }

        public static void Update(this Instrument entry)
        {
            // Updates a given Instrument entry

            using (DBEntities entities = new DBEntities())
            {
                Instrument tempEntry = entities.Instruments.First(inst => inst.ID == entry.ID);

                entities.Entry(tempEntry).CurrentValues.SetValues(entities);

                entities.SaveChanges();
            }
        }

        #endregion
        
    }
}
