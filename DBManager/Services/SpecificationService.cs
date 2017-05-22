using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager.Services
{
    public static class SpecificationService
    {
        #region Operations for ControlPlans entities

        public static ControlPlan GetControlPlan(int ID)
        {
            using (DBEntities entities = new DBEntities())
            {
                return entities.ControlPlans.First(entry => entry.ID == ID);
            }
        }

        public static IEnumerable<ControlPlan> GetControlPlans()
        {
            using (var entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;
                return entities.ControlPlans.Include(cpl => cpl.Specification)
                                            .OrderBy(cpl => cpl.Specification.Standard.Name)
                                            .ToList();
            }
        }

        public static void Create(this ControlPlan entry)
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.ControlPlans.Attach(entry);
                entities.Entry(entry).State = System.Data.Entity.EntityState.Added;
                entities.SaveChanges();
            }
        }

        public static void Delete(this ControlPlan entry)
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.ControlPlans.Attach(entry);
                entities.Entry(entry).State = System.Data.Entity.EntityState.Deleted;
                entities.SaveChanges();
            }
        }

        public static void Load(this ControlPlan entry)
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                entities.ControlPlans.Attach(entry);

                ControlPlan tempEntry = entities.ControlPlans.Include(cpl => cpl.Specification)
                                                            .Include(cpl => cpl.ControlPlanItems)
                                                            .Include(cpl => cpl.ControlPlanItems
                                                            .Select(cpi => cpi.Requirement))
                                                            .First(spec => spec.ID == entry.ID);

                entities.Entry(entry).CurrentValues.SetValues(tempEntry);
            }
        }

        public static void Update(this ControlPlan entry)
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.ControlPlans.Attach(entry);
                entities.Entry(entry).State = System.Data.Entity.EntityState.Modified;
                entities.SaveChanges();
            }
        }

        #endregion

        #region Operations for Method items

        public static Method GetMethod (int ID)
        {
            using (DBEntities entities = new DBEntities())
            {
                return entities.Methods.First(entry => entry.ID == ID);
            }
        }

        public static IEnumerable<Method> GetMethods()
        {
            using (var entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Methods.Include(mtd => mtd.Standard)
                                        .Include(mtd => mtd.Property)
                                        .OrderBy(spec => spec.Standard.Name)
                                        .ToList();
            }
        }

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

        #endregion

        #region Operations for Specification entities

        public static Specification GetSpecification(int ID)
        {
            using (DBEntities entities = new DBEntities())
            {
                return entities.Specifications.First(entry => entry.ID == ID);
            }
        }

        public static IEnumerable<Specification> GetSpecifications()
        {
            using (var entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;
                return entities.Specifications.Include(spec => spec.Standard)
                                                .Include(spec => spec.Standard.CurrentIssue)
                                                .Include(spec => spec.Standard.Organization)
                                                .OrderBy(spec => spec.Standard.Name)
                                                .ToList();
            }
        }

        public static void Create(this Specification entry)
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Specifications.Attach(entry);
                entities.Entry(entry).State = System.Data.Entity.EntityState.Added;
                entities.SaveChanges();
            }
        }

        public static void Delete(this Specification entry)
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Specifications.Attach(entry);
                entities.Entry(entry).State = System.Data.Entity.EntityState.Deleted;
                entities.SaveChanges();
            }
        }

        public static void Load(this Specification entry)
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                entities.Specifications.Attach(entry);

                Specification tempEntry = entities.Specifications.Include(spec => spec.SpecificationVersions)
                                                                .Include(spec => spec.Standard)
                                                                .Include(spec => spec.Standard.CurrentIssue)
                                                                .Include(spec => spec.ControlPlans)
                                                                .First(spec => spec.ID == entry.ID);

                entities.Entry(entry).CurrentValues.SetValues(tempEntry);
            }
        }

        public static void Update(this Specification entry)
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Specifications.Attach(entry);
                entities.Entry(entry).State = System.Data.Entity.EntityState.Modified;
                entities.SaveChanges();
            }
        }

        #endregion
    }
}
