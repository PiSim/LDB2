using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager.EntityExtensions
{
    public static class MethodExtension
    {

        public static void Create(this Method entry)
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Methods.Attach(entry);
                entities.Entry(entry).State = System.Data.Entity.EntityState.Added;
                entities.SaveChanges();
            }
        }

        public static void Delete(this Method entry)
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Methods.Attach(entry);
                entities.Entry(entry).State = System.Data.Entity.EntityState.Deleted;
                entities.SaveChanges();
            }
        }

        public static void Load(this Method entry)
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                entities.Methods.Attach(entry);

                Method tempEntry = entities.Methods.Include(mtd => mtd.AssociatedInstruments
                                                    .Select(inst => inst.InstrumentType))
                                                    .Include(mtd => mtd.ExternalReports
                                                    .Select(extr => extr.ExternalLab))
                                                    .Include(mtd => mtd.Property)
                                                    .Include(mtd => mtd.Standard)
                                                    .Include(mtd => mtd.SubMethods)
                                                    .Include(mtd => mtd.Tests
                                                    .Select(tst => tst.SubTests))
                                                    .Include(mtd => mtd.Tests
                                                    .Select(tst => tst.Report))
                                                    .First(spec => spec.ID == entry.ID);

                entities.Entry(entry).CurrentValues.SetValues(tempEntry);
            }
        }

        public static void LoadSubMethods(this Method entry)
        {
            // Loads related sub methods in a given method entry

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                Method tempEntry = entities.Methods.Include(mtd => mtd.SubMethods)
                                                    .First(mtd => mtd.ID == entry.ID);

                entry.SubMethods = tempEntry.SubMethods;
            }
        }

        public static void Update(this Method entry)
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.AutoDetectChangesEnabled = false;

                Method tempEntry = entities.Methods.First(mtd => mtd.ID == entry.ID);
                entities.Entry(tempEntry).CurrentValues.SetValues(entry);
                entities.Entry(tempEntry).State = EntityState.Modified;
                entities.SaveChanges();
            }
        }
    }
}
