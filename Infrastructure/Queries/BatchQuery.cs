using DataAccess;
using LabDbContext;
using System.Linq;

namespace Infrastructure.Queries
{
    /// <summary>
    /// Query object returning the first batch with a given ID or Number
    /// If both ID and Number are given for the search, precedence is given to the ID
    /// ID : The ID of the batch
    /// Number : the Number of the batch
    /// </summary>
    public class BatchQuery : IScalar<Batch, LabDbEntities>
    {
        #region Properties

        public int? ID { get; set; } = null;

        public string Number { get; set; } = null;

        #endregion Properties

        #region Methods

        public Batch Execute(LabDbEntities context)
        {
            IQueryable<Batch> query = context.Batches;

            if (ID != null)
                return query.FirstOrDefault(btc => btc.ID == ID);

            return query.FirstOrDefault(btc => btc.Number == Number);
        }

        #endregion Methods
    }
}