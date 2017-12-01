using DBManager;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Queries
{
    public class BatchesNotArrivedQuery : IQuery<Batch>
    {
        public IQueryable<Batch> RunQuery(DBEntities entities)
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
                                    .Where(btc => !btc.FirstSampleArrived)
                                    .OrderByDescending(btc => btc.Number);
        }
    }
}
