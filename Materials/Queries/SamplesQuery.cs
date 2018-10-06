using DataAccess;
using LabDbContext;
using System.Linq;

namespace Materials.Queries
{
    /// <summary>
    /// Query object that returns Samples
    /// </summary>
    public class SamplesQuery : IQuery<Sample, LabDbEntities>
    {
        #region Properties

        /// <summary>
        /// Sets a BatchID to use as filter
        /// </summary>
        public int? BatchID { get; set; }

        /// <summary>
        /// If True the results are ordered by date descending
        /// </summary>
        public bool OrderResults { get; set; }

        #endregion Properties

        #region Methods

        public IQueryable<Sample> Execute(LabDbEntities context)
        {
            IQueryable<Sample> query = context.Samples;

            if (BatchID != null)
                query = query.Where(smp => smp.BatchID == BatchID);

            if (OrderResults)
                query = query.OrderByDescending(smp => smp.Date);

            return query;
        }

        #endregion Methods
    }
}