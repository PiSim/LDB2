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
            // Returns all Method entities.

            using (var entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Methods.Include(mtd => mtd.Standard.Organization)
                                        .Include(mtd => mtd.Property)
                                        .Where(mtd => true)
                                        .OrderBy(mtd => mtd.Standard.Name)
                                        .ToList();
            }
        }

        #endregion

        #region Operations for Property entities

        public static IEnumerable<Property> GetProperties()
        {
            // Returns all property entities

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Properties.ToList();
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

        public static void Load(this Specification entry)
        {
            // Loads all relevant Related entities into a given Specification entry

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                entities.Specifications.Attach(entry);

                Specification tempEntry = entities.Specifications.Include(spec => spec.SpecificationVersions)
                                                                .Include(spec => spec.Standard)
                                                                .Include(spec => spec.Standard.CurrentIssue)
                                                                .Include(spec => spec.Standard.StandardIssues)
                                                                .Include(spec => spec.Standard.Organization)
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

        #region Operations for SpecificationVersion entities

        public static void Delete(this SpecificationVersion entry)
        {
            // Deletes SpecificationVersion entity
            {
                using (DBEntities entities = new DBEntities())
                {
                    entities.SpecificationVersions.Attach(entry);
                    entities.Entry(entry).State = EntityState.Deleted;
                    entities.SaveChanges();
                }
            }
        }


        #endregion

        #region Operations for StandardFiles entities

        public static void Delete(this StandardFile entry)
        {
            // Deletes StandardFile entry

            using (DBEntities entities = new DBEntities())
            {
                entities.StandardFiles.Attach(entry);
                entities.Entry(entry).State = EntityState.Deleted;
                entities.SaveChanges();
            }
        }

        #endregion

        #region Operations for StandardIssue entities

        public static void Delete(this StandardIssue entry)
        {
            // Deletes StandardIssue entry

            using (DBEntities entities = new DBEntities())
            {
                entities.StandardIssues.Attach(entry);
                entities.Entry(entry).State = EntityState.Deleted;
                entities.SaveChanges();
            }
        }

        public static void Load(this StandardIssue entry)
        {
            // Loads relevant RelatedEntities for StandardIssue entry

            if (entry == null)
                return;

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                StandardIssue tempEntry = entities.StandardIssues.Include(stdi => stdi.StandardFiles)
                                                                .First(stdi => stdi.ID == entry.ID);

                entities.Entry(entry).CurrentValues.SetValues(tempEntry);
                entities.SaveChanges();
            }

        }

        #endregion

        #region Operations for Std entities

        public static Std GetStandard(string name)
        {
            // returns Standard entity with the provided name or null if none is found

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Stds.Include(std => std.Organization)
                                    .Include(std => std.CurrentIssue)
                                    .FirstOrDefault(std => std.Name == name);
            }
        }

        #endregion
    }
}
