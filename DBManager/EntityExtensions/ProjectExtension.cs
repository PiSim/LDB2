using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager.EntityExtensions
{
    public static class ProjectExtension
    {
        public static IEnumerable<Construction> GetConstructions(this Project entry)
        {
            // Returns all Construction entities for a Project

            if (entry == null)
                return null;

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Constructions.Include(con => con.Aspect)
                                            .Include(con => con.ExternalConstruction)
                                            .Include(con => con.Type)
                                            .Where(con => con.ProjectID == entry.ID)
                                            .ToList();
            }
        }
    }
}
