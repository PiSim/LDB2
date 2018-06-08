using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager
{
    public partial class Specification
    {

        public ControlPlan AddControlPlan(bool asDefault = false)
        {
            // Generates a new control plan for this specification

            using (DBEntities entities = new DBEntities())
            {
                ControlPlan newEntry = new ControlPlan()
                {
                    IsDefault = asDefault,
                    Name = (asDefault) ? "Completo" : "Nuovo Piano di Controllo",
                    SpecificationID = ID
                };

                SpecificationVersion mainVersion = entities.SpecificationVersions
                                                            .FirstOrDefault(spv => spv
                                                            .SpecificationID == ID &&
                                                            spv.IsMain);

                if (mainVersion == null)
                    throw new InvalidOperationException();

                foreach (Requirement req in mainVersion.Requirements)
                    newEntry.control_plan_items_b.Add(new ControlPlanItem()
                    {
                        IsSelected = asDefault,
                        RequirementID = req.ID
                    });

                entities.ControlPlans.Add(newEntry);

                entities.SaveChanges();

                return newEntry;
            }
        }

        public void AddMethod(Requirement requirementEntry)
        {
            // Adds a requirement to a Specification

            using (DBEntities entities = new DBEntities())
            {
                entities.SpecificationVersions
                        .First(spcv => spcv.SpecificationID == ID && spcv.IsMain)
                        .Requirements
                        .Add(requirementEntry);

                foreach (ControlPlan cp in entities.ControlPlans.Where(cp => cp.SpecificationID == ID))
                    requirementEntry.ControlPlanItems
                                    .Add(new ControlPlanItem()
                    {
                        ControlPlan = cp,
                        IsSelected = cp.IsDefault
                    });

                entities.SaveChanges();
            }            
        }

        public IList<ControlPlan> GetControlPlans()
        {
            // Returns all existing control plans for a specification

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.ControlPlans
                                .Where(cp => cp.SpecificationID == ID)
                                .ToList();
            }
        }

        public IList<SpecificationVersion> GetVersions()
        {
            //Returns all version for a given specification entry

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.SpecificationVersions.Where(specv => specv.SpecificationID == ID)
                                                    .ToList();
            }
        }

        public double TotalWorkHours
        {
            get
            {
                using (DBEntities entities = new DBEntities())
                {
                    return entities.SpecificationVersions
                                    .First(spcv => spcv.SpecificationID == ID && spcv.IsMain)
                                    .Requirements
                                    .Sum(req => req.Method.Duration);
                }
            }
        }
    }

    public static class SpecificationExtension
    {

        public static void Create(this Specification entry)
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Specifications.Add(entry);
                entities.SaveChanges();
            }
        }

        public static void Delete(this Specification entry)
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Entry(entities.Specifications.First(spec => spec.ID == entry.ID))
                                                        .State = EntityState.Deleted;
                entities.SaveChanges();
            }
        }

        public static IEnumerable<StandardFile> GetFiles(this Specification entry)
        {
            // Returns all standard files for specification standard

            if (entry == null)
                return new List<StandardFile>();

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.StandardFiles
                                .Where(stdf => stdf.StandardID == entry.StandardID)
                                .ToList();
            }
        }

        public static IEnumerable<Requirement> GetMainVersionRequirements(this Specification entry)
        {
            // Returns the requirement list of the Specification entry's main version

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Requirements.Include(req => req.Method.Property)
                                            .Include(req => req.Method.Standard.Organization)
                                            .Include(req => req.SubRequirements
                                            .Select(sreq => sreq.SubMethod))
                                            .Where(req => req.SpecificationVersionID == entities.SpecificationVersions
                                            .FirstOrDefault(specv => specv.SpecificationID == entry.ID && specv.IsMain).ID)
                                            .ToList();
            }
        }

        public static IEnumerable<Report> GetReports(this Specification entry)
        {
            // Returns all Report entities for a given Specification entry

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Reports.Include(rep => rep.Author)
                                        .Include(rep => rep.Batch.Material.Aspect)
                                        .Include(rep => rep.Batch.Material.MaterialLine)
                                        .Include(rep => rep.Batch.Material.MaterialType)
                                        .Include(rep => rep.Batch.Material.Project.Oem)
                                        .Include(rep => rep.Batch.Material.Recipe.Colour)
                                        .Include(rep => rep.SpecificationVersion.Specification.Standard)
                                        .Where(rep => rep.SpecificationVersion.Specification.ID == entry.ID)
                                        .ToList();
            }
        }

        public static void Load(this Specification entry)
        {
            // Loads all relevant Related entities into a given Specification entry

            if (entry == null)
                return;

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;
                
                Specification tempEntry = entities.Specifications.Include(spec => spec.Standard.Organization)
                                                                .First(spec => spec.ID == entry.ID);
                
                entry.Standard = tempEntry.Standard;
                entry.StandardID = tempEntry.StandardID;
            }
        }

        public static void Update(this Specification entry)
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Specifications.AddOrUpdate(entry);
                entities.SaveChanges();
            }
        }
    }
}
