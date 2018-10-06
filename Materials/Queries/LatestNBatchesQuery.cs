using DataAccess;
using LabDbContext;
using System.Data.Entity;
using System.Linq;

namespace Materials.Queries
{
    public class LatestNBatchesQuery : IQuery<Batch, LabDbEntities>
    {
        #region Fields

        private int _entryNumber;

        #endregion Fields

        #region Constructors

        public LatestNBatchesQuery(int entryNumber = 25)
        {
            _entryNumber = entryNumber;
        }

        #endregion Constructors

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
                                    .OrderByDescending(btc => btc.Number)
                                    .Take(_entryNumber);
        }

        #endregion Methods
    }
}