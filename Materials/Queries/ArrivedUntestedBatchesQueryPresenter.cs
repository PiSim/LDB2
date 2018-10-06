using DataAccess;
using LabDbContext;

namespace Materials.Queries
{
    public class ArrivedUntestedBatchesQueryPresenter : IQueryPresenter<Batch, LabDbEntities>
    {
        #region Properties

        public string Description { get; } = "Restituisce una lista di Batch per cui è stato registrato l'arrivo di almeno un rotolo, " +
                                                "che non hanno Report e che non sono flaggati come da non testare";

        public string Name { get; } = "Batch arrivati ma non testati";
        public IQuery<Batch, LabDbEntities> Query => new ArrivedUntestedBatchesQuery();

        #endregion Properties
    }
}