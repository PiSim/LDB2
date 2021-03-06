﻿using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace LabDbContext
{
    public static class ExternalConstructionExtension
    {
        #region Methods

        public static IEnumerable<Material> GetMaterials(this ExternalConstruction entry)
        {
            // Returns all materials for an external construction

            if (entry == null)
                return null;

            using (LabDbEntities entities = new LabDbEntities())
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

        #endregion Methods
    }

    public partial class ExternalConstruction
    {
        #region Methods

        public IEnumerable<Batch> GetBatches(LabDbEntities entities)
        {
            entities.Configuration.LazyLoadingEnabled = false;

            return entities.Batches.Include(btc => btc.Material.Aspect)
                                    .Include(btc => btc.Material.MaterialLine)
                                    .Include(btc => btc.Material.MaterialType)
                                    .Include(btc => btc.Material.Recipe.Colour)
                                    .Where(btc => btc.Material.ExternalConstructionID == ID)
                                    .ToList();
        }

        #endregion Methods
    }
}