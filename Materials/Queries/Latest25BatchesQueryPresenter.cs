using DataAccess;
using LabDbContext;

namespace Materials.Queries
{
    public class Latest25BatchesQueryPresenter : IQueryPresenter<Batch, LabDbEntities>
    {
        #region Properties

        public string Description => "Restituisce gli ultimi 25 Batch in ordine di numero";
        public string Name => "Ultimi 25 Batch";
        public IQuery<Batch, LabDbEntities> Query => new LatestNBatchesQuery();

        #endregion Properties
    }
}