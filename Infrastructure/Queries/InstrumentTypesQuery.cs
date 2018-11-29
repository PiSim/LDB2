using DataAccessCore;
using LInst;
using System.Data.Entity;
using System.Linq;

namespace Infrastructure.Queries
{
    /// <summary>
    /// Query object that returns multiple InstrumentType entities
    /// </summary>
    public class InstrumentTypesQuery : IQuery<InstrumentType, LInstContext>
    {
        #region Properties

        /// <summary>
        /// If true the query is run AsNoTracking
        /// </summary>
        public bool AsNoTracking { get; set; } = true;

        /// <summary>
        /// If true the results are ordered by name
        /// </summary>
        public bool OrderResults { get; set; } = true;

        #endregion Properties

        #region Methods

        public IQueryable<InstrumentType> Execute(LInstContext context)
        {
            IQueryable<InstrumentType> query = context.InstrumentTypes;

            if (AsNoTracking)
                query = query.AsNoTracking();

            if (OrderResults)
                query = query.OrderBy(it => it.Name);

            return query;
        }

        #endregion Methods
    }
}