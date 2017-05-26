using System;
using System.Collections.Generic;
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
    }
}
