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

        #region Operations for Requirement entities

        public static void Create(this Requirement entry)
        {
            // Insert new Requirement entry in the DB

            using (DBEntities entities = new DBEntities())
            {
                entities.Requirements.Add(entry);
                entities.SaveChanges();
            }
        }

        public static void Delete(this Requirement entry)
        {
            // Deletes Requirement entry

            using (DBEntities entities = new DBEntities())
            {
                entities.Requirements.Attach(entry);
                entities.Entry(entry).State = EntityState.Deleted;
                entities.SaveChanges();
            }
        }

        #endregion

        #region Operations for Specification entities

        public static void AddRequirement(this Specification entry,
                                        Requirement requirementEntity)
        {
            // Creates a new Test entity adding it to the specification entry

            using (DBEntities entities = new DBEntities())
            {
                entities.Specifications.Attach(entry);
                entry.SpecificationVersions.First(specv => specv.IsMain)
                                            .Requirements
                                            .Add(requirementEntity);

                entities.SaveChanges();
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

        public static IEnumerable<StandardIssue> GetIssues(this Specification entry)
        {
            // Returns all Issue entities for a given Specification entry

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entry.Standard.StandardIssues.ToList();
            }
        }

        public static IEnumerable<Report> GetReports(this Specification entry)
        {
            // Returns all Report entities for a given Specification entry

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Reports.Include(rep => rep.Author)
                                        .Include(rep => rep.Batch.Material.Construction.Aspect)
                                        .Include(rep => rep.Batch.Material.Construction.Project.Oem)
                                        .Include(rep => rep.Batch.Material.Construction.Type)
                                        .Include(rep => rep.Batch.Material.Recipe.Colour)
                                        .Include(rep => rep.SpecificationVersion.Specification.Standard.CurrentIssue)
                                        .Where(rep => rep.SpecificationVersion.Specification.ID == entry.ID)
                                        .ToList();
            }
        }

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

        public static void Load(this SpecificationVersion entry)
        {
            // Loads relevant RelatedEntities for given SpecificationVersion entry

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                entities.SpecificationVersions.Attach(entry);

                SpecificationVersion tempEntry = entities.SpecificationVersions.Include(specv => specv.ExternalConstructions)
                                                                                .Include(specv => specv.Requirements
                                                                                .Select(req => req.SubRequirements))
                                                                                .Include(specv => specv.Requirements
                                                                                .Select(req => req.Overridden))
                                                                                .Include(specv => specv.Requirements
                                                                                .Select(req => req.Method.Property))
                                                                                .Include(specv => specv.Requirements
                                                                                .Select(req => req.Method.Standard.Organization))
                                                                                .Include(req => req.Specification.Standard.Organization)
                                                                                .Include(req => req.Specification.Standard.CurrentIssue)
                                                                                .First(specv => specv.ID == entry.ID);

                entities.Entry(entry).CurrentValues.SetValues(tempEntry);
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
