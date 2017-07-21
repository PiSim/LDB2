using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager.Services
{
    public static class SpecificationService
    {
        public static Requirement GetRequirement(int ID)
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Requirements.First(req => req.ID == ID);
            }
        }

        public static Specification GetSpecification(int ID)
        {
            using (DBEntities entities = new DBEntities())
            {
                return entities.Specifications.First(entry => entry.ID == ID);
            }
        }

        public static Specification GetSpecification(string name)
        {
            // returns a specification with the given Standard name, or null if none is found
            
            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Specifications.FirstOrDefault(spec => spec.Standard.Name == name);
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
                                                .Where(spec => true)
                                                .OrderBy(spec => spec.Standard.Name)
                                                .ToList();
            }
        }

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
            if (entry == null)
                return;

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                ControlPlan tempEntry = entities.ControlPlans.Include(cpl => cpl.Specification)
                                                            .Include(cpl => cpl.ControlPlanItems)
                                                            .Include(cpl => cpl.ControlPlanItems
                                                            .Select(cpi => cpi.Requirement))
                                                            .First(spec => spec.ID == entry.ID);

                entry.ControlPlanItems = tempEntry.ControlPlanItems;
                entry.IsDefault = tempEntry.IsDefault;
                entry.Name = tempEntry.Name;
                entry.Specification = tempEntry.Specification;
                entry.SpecificationID = tempEntry.SpecificationID;
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
        
        
        
        public static void UpdateRequirements(IEnumerable<Requirement> requirementEntries)
        {
            // Updates all the Requirement entries passed as parameter

            using (DBEntities entities = new DBEntities())
            {
                foreach (Requirement req in requirementEntries)
                {
                    entities.Requirements.AddOrUpdate(req);
                    foreach (SubRequirement sreq in req.SubRequirements)
                        entities.SubRequirements.AddOrUpdate(sreq);
                }

                entities.SaveChanges();
            }
        }

        public static void UpdateSubMethods(IEnumerable<SubMethod> methodEntries)
        {
            // Updates all the SubMethod entries

            using (DBEntities entities = new DBEntities())
            {
                foreach (SubMethod smtd in methodEntries)
                    entities.SubMethods.AddOrUpdate(smtd);

                entities.SaveChanges();
            }
        }

        public static SpecificationVersion GetSpecificationVersion(int ID)
        {

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.SpecificationVersions.FirstOrDefault(specv => specv.ID == ID);
            }

        }
    }
}
