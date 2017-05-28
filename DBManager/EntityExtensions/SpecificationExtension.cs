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

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entry.Standard.StandardIssues.ToList();
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

    }
}
