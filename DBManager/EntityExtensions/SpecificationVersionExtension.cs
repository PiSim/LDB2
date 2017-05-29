using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager.EntityExtensions
{
    public static class SpecificationVersionExtension
    {
        public static void AddRequirement(this SpecificationVersion entry,
                                        Requirement requirementEntry)
        {
            // Adds a Requirement entity to a specificationVersion requirement list

            entry.Requirements.Add(requirementEntry);
            requirementEntry.SpecificationVersionID = entry.ID;

            using (DBEntities entities = new DBEntities())
            {
                entities.Requirements.Add(requirementEntry);
                entities.SaveChanges();
            }
        }

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

        public static IEnumerable<Requirement> GenerateRequirementList(this SpecificationVersion version)
        {
            if (version == null)
                return new List<Requirement>();

            if (version.IsMain)
                return version.GetRequirements();

            else
            {
                List<Requirement> output = new List<Requirement>(
                    version.Specification.GetMainVersionRequirements());

                foreach (Requirement requirement in version.GetRequirements())
                {
                    int ii = output.FindIndex(rr => rr.Method.ID == requirement.Method.ID);
                    output[ii] = requirement;
                }

                return output;
            }
        }

        public static IEnumerable<Requirement> GetRequirements(this SpecificationVersion entry)
        {
            // returns loaded requirement list for version

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Requirements.Include(req => req.Method.Property)
                                            .Include(req => req.Method.Standard.Organization)
                                            .Include(req => req.SubRequirements
                                            .Select(sreq => sreq.SubMethod))
                                            .Where(req => req.SpecificationVersionID == entry.ID)
                                            .ToList();
            }
        }


        public static void Load(this SpecificationVersion entry)
        {
            // Loads relevant RelatedEntities for given SpecificationVersion entry

            if (entry == null)
                return;

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;
                

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

                entry.ExternalConstructions = tempEntry.ExternalConstructions;
                entry.IsMain = tempEntry.IsMain;
                entry.Name = tempEntry.Name;
                entry.Reports = tempEntry.Reports;
                entry.Requirements = tempEntry.Requirements;
                entry.Specification = tempEntry.Specification;
                entry.SpecificationID = tempEntry.SpecificationID;
                entry.Tasks = tempEntry.Tasks;
            }
        }

    }
}
