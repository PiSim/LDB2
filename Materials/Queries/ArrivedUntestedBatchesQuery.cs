﻿using DataAccess;
using LabDbContext;
using System.Data.Entity;
using System.Linq;

namespace Materials.Queries
{
    /// <summary>
    /// Queries the DB for the Arrived batches without a report not marked as DoNotTest
    /// </summary>
    public class ArrivedUntestedBatchesQuery : IQuery<Batch, LabDbEntities>
    {
        #region Methods

        public IQueryable<Batch> Execute(LabDbEntities entities)
        {
            return entities.Batches.Include(btc => btc.BasicReport)
                                    .Include(btc => btc.FirstSample)
                                    .Include(btc => btc.LatestSample)
                                    .Include(btc => btc.Material.Aspect)
                                    .Include(btc => btc.Material.ExternalConstruction.Oem)
                                    .Include(btc => btc.Material.MaterialLine)
                                    .Include(btc => btc.Material.MaterialType)
                                    .Include(btc => btc.Material.Project.Oem)
                                    .Include(btc => btc.Material.Recipe.Colour)
                                    .Include(btc => btc.TrialArea)
                                    .Where(btc => btc.FirstSampleArrived)
                                    .Where(btc => !btc.DoNotTest)
                                    .Where(btc => btc.BasicReport == null)
                                    .OrderByDescending(btc => btc.Number);
        }

        #endregion Methods
    }
}