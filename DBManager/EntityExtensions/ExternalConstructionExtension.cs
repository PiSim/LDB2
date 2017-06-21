using DBManager;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager.EntityExtensions
{
    public static class ExternalConstructionExtension
    {
        public static IEnumerable<Material> GetMaterials(this ExternalConstruction entry)
        {
            // Returns all materials for an external construction

            if (entry == null)
                return null;

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Materials.Include(mat => mat.Aspect)
                                        .Include(mat => mat.MaterialLine)
                                        .Include(mat => mat.MaterialType)
                                        .Include(mat => mat.Recipe.Colour)
                                        .Where(mat => mat.ExternalConstructionID == entry.ID)
                                        .ToList();
            }
        }
    }
}
