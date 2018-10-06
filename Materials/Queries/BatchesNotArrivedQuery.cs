using DataAccess;
using LabDbContext;
using System.Data.Entity;
using System.Linq;

namespace Materials.Queries
{
    public class BatchesNotArrivedQuery : IQuery<Batch, LabDbEntities>
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
                                    .Where(btc => !btc.FirstSampleArrived)
                                    .OrderByDescending(btc => btc.Number);
        }

        #endregion Methods
    }
}