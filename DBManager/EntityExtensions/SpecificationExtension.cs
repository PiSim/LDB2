using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager.EntityExtensions
{
    public static class SpecificationExtension
    {
        public static void AddMethod(this Specification entry,
                                    Requirement requirementEntry)
        {
            // Adds a requirement generated to a Specification's Main Version
            
            entry.SpecificationVersions.FirstOrDefault(spcv => spcv.IsMain)
                                        .AddRequirement(requirementEntry);
        }

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

        public static IEnumerable<StandardIssue> GetIssues(this Specification entry)
        {
            // Returns all Issue entities for a given Specification entry

            if (entry == null)
                return new List<StandardIssue>();

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.StandardIssues.Where(stdi => stdi.StandardID == entry.StandardID)
                                                .ToList();
            }
        }

        public static IEnumerable<Requirement> GetMainVersionRequirements(this Specification entry)
        {
            // Returns the requirement list of the Specification entry's main version

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Requirements.Include(req => req.Method.Standard.Organization)
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
                                        .Include(rep => rep.Batch.Material.Construction.Aspect)
                                        .Include(rep => rep.Batch.Material.Construction.Project.Oem)
                                        .Include(rep => rep.Batch.Material.Construction.Type)
                                        .Include(rep => rep.Batch.Material.Recipe.Colour)
                                        .Include(rep => rep.SpecificationVersion.Specification.Standard.CurrentIssue)
                                        .Where(rep => rep.SpecificationVersion.Specification.ID == entry.ID)
                                        .ToList();
            }
        }

        public static void SetCurrentIssue(this Specification entry,
                                            StandardIssue issueEntity)
        {
            // Sets the current issue of a specification's standard

            using (DBEntities entities = new DBEntities())
            {
                Specification tempSpec = entities.Specifications.Include(spec => spec.Standard.StandardIssues)
                                                                .First(spec => spec.ID == entry.ID);

                tempSpec.Standard.CurrentIssueID = issueEntity.ID;
                                foreach (StandardIssue stdi in tempSpec.Standard.StandardIssues)
                {
                    if (stdi.ID == issueEntity.ID)
                        stdi.IsCurrent = true;

                    else
                        stdi.IsCurrent = false;
                }

                entities.SaveChanges();
            }
        }

        public static void Load(this Specification entry)
        {
            // Loads all relevant Related entities into a given Specification entry

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;
                
                Specification tempEntry = entities.Specifications.Include(spec => spec.SpecificationVersions)
                                                                .Include(spec => spec.Standard)
                                                                .Include(spec => spec.Standard.CurrentIssue)
                                                                .Include(spec => spec.Standard.StandardIssues)
                                                                .Include(spec => spec.Standard.Organization)
                                                                .Include(spec => spec.ControlPlans)
                                                                .First(spec => spec.ID == entry.ID);

                entry.ControlPlans = tempEntry.ControlPlans;
                entry.Description = tempEntry.Description;
                entry.SpecificationVersions = tempEntry.SpecificationVersions;
                entry.Standard = tempEntry.Standard;
                entry.StandardID = tempEntry.StandardID;
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

    }
}
