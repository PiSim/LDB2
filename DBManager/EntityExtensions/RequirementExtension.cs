using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager.EntityExtensions
{
    public static class RequirementExtension
    {
        public static void Load(this Requirement entry)
        {
            // Loads DB values a given Requirement entry

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                entities.Requirements.Attach(entry);

                Requirement tempEntry = entities.Requirements.Include(req => req.Method.Property)
                                                                .Include(req => req.Method.Standard.CurrentIssue)
                                                                .Include(req => req.Method.Standard.Organization)
                                                                .Include(req => req.Overridden)
                                                                .Include(req => req.SubRequirements
                                                                .Select(sreq => sreq.SubMethod.Method))
                                                                .First(req => req.ID == entry.ID);

                entities.Entry(entry).CurrentValues.SetValues(tempEntry);
            }
        }
    }
}
