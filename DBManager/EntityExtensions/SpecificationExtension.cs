using System;
using System.Collections.Generic;
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
    }
}
