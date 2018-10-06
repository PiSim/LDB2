using DataAccess;
using LabDbContext;
using System.Linq;

namespace Materials.Queries
{
    /// <summary>
    /// Query Object that returns all batches with non zero ArchiveStock
    /// </summary>
    public class SampleArchiveQuery : IQuery<Batch, LabDbEntities>
    {
        #region Methods

        public IQueryable<Batch> Execute(LabDbEntities context)
        {
            return context.Batches.Where(btc => btc.ArchiveStock > 0)
                                    .OrderByDescending(btc => btc.Number);
        }

        #endregion Methods
    }
}