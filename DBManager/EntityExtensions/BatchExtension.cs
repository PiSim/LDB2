using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager
{
    public static class BatchExtension
    {
        public static Material GetMaterial(this Batch entry)
        {
            // Returns loaded material for batch entry

            if (entry == null)
                return null;

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Materials.Include(mat => mat.Construction.Aspect)
                                        .Include(mat => mat.Construction.Type)
                                        .Include(mat => mat.Construction.ExternalConstruction)
                                        .Include(mat => mat.Recipe.Colour)
                                        .FirstOrDefault(mat => mat.ID == entry.MaterialID);
            }
        }
    }
}
