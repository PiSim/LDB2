using DBManager;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Queries
{
    public class LatestNBatchesQuery : IQuery<Batch>
    {
        private int _entryNumber;

        public LatestNBatchesQuery(int entryNumber = 25)
        {
            _entryNumber = entryNumber;
        }

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
                                    .OrderByDescending(btc => btc.Number)
                                    .Take(_entryNumber);
        }
    }
}
