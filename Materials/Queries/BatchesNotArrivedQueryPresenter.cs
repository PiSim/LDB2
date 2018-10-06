using DataAccess;
using LabDbContext;

namespace Materials.Queries
{
    public class BatchesNotArrivedQueryPresenter : IQueryPresenter<Batch, LabDbEntities>
    {
        #region Properties

        public string Description => "Restituisce i Batch di cui non sono stati ricevuti campioni";
        public string Name => "Batch Non Arrivati";
        public IQuery<Batch, LabDbEntities> Query => new BatchesNotArrivedQuery();

        #endregion Properties
    }
}