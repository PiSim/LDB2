using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace LabDbContext
{
    public static class SpecificationVersionExtension
    {
        #region Methods

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