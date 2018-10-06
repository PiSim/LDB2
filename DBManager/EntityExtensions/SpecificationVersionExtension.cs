using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace LabDbContext
{
    public static class SpecificationVersionExtension
    {
        #region Methods

        public static void AddRequirement(this SpecificationVersion entry,
                                        Requirement requirementEntry)
        {
            // Adds a Requirement entity to a specificationVersion requirement list

            entry.Requirements.Add(requirementEntry);
            requirementEntry.SpecificationVersionID = entry.ID;

            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.Requirements.Add(requirementEntry);
                entities.SaveChanges();
            }
        }

        public static void Create(this SpecificationVersion entry)
        {
            // Inserts a new SpecificationVersion entry in the DB

            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.SpecificationVersions.Add(entry);

                entities.SaveChanges();
            }
        }

        public static void Delete(this SpecificationVersion entry)
        {
            // Deletes SpecificationVersion entity
            {
                using (LabDbEntities entities = new LabDbEntities())
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
                    version.GetMainVersionRequirements());

                foreach (Requirement requirement in version.GetRequirements())
                {
                    int ii = output.FindIndex(rr => rr.MethodVariantID == requirement.MethodVariantID);
                    output[ii] = requirement;
                }

                return output;
            }
        }

        public static IEnumerable<Requirement> GetRequirements(this SpecificationVersion entry)
        {
            // returns loaded requirement list for version

            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Requirements.Include(req => req.MethodVariant.Method.Property)
                                            .Include(req => req.MethodVariant.Method.Standard.Organization)
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

            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                SpecificationVersion tempEntry = entities.SpecificationVersions.Include(specv => specv.ExternalConstructions)
                                                                                .Include(specv => specv.Requirements
                                                                                .Select(req => req.SubRequirements))
                                                                                .Include(specv => specv.Requirements
                                                                                .Select(req => req.Overridden))
                                                                                .Include(specv => specv.Requirements
                                                                                .Select(req => req.MethodVariant.Method.Property))
                                                                                .Include(specv => specv.Requirements
                                                                                .Select(req => req.MethodVariant.Method.Standard.Organization))
                                                                                .Include(req => req.Specification.Standard.Organization)
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

        public static void Update(this SpecificationVersion entry)
        {
            // Updates a SpcificationVersion Entry

            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.SpecificationVersions.AddOrUpdate(entry);

                entities.SaveChanges();
            }
        }

        #endregion Methods
    }

    public partial class SpecificationVersion
    {
        #region Properties

        public string VersionString => Specification?.Standard?.Name + " " + Name;

        #endregion Properties

        #region Methods

        public IEnumerable<Requirement> GetMainVersionRequirements()
        {
            // Returns the requirement list of the Specification entry's main version

            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Requirements.Include(req => req.MethodVariant.Method.Property)
                                            .Include(req => req.MethodVariant.Method.Standard.Organization)
                                            .Include(req => req.SubRequirements
                                            .Select(sreq => sreq.SubMethod))
                                            .Where(req => req.SpecificationVersionID == entities.SpecificationVersions
                                            .FirstOrDefault(specv => specv.SpecificationID == SpecificationID && specv.IsMain).ID)
                                            .ToList();
            }
        }

        #endregion Methods
    }
}