using DataAccessCore;
using LInst;
using System.Data.Entity;
using System.Linq;

namespace Infrastructure.Queries
{
    /// <summary>
    /// Query object that returns multiple InstrumentUtilizationArea entities
    /// </summary>
    public class InstrumentUtilizationAreasQuery : IQuery<InstrumentUtilizationArea, LInstContext>
    {
        #region Properties

        /// <summary>
        /// If true the query is run AsNoTracking
        /// </summary>
        public bool AsNoTracking { get; set; } = true;

        /// <summary>
        /// If true the results are ordered by code
        /// </summary>
        public bool OrderResults { get; set; } = true;

        #endregion Properties

        #region Methods

        public IQueryable<InstrumentUtilizationArea> Execute(LInstContext context)
        {
            IQueryable<InstrumentUtilizationArea> query = context.InstrumentUtilizationAreas;

            if (AsNoTracking)
                query = query.AsNoTracking();

            if (OrderResults)
                query = query.OrderBy(iua => iua.Name);

            return query;
        }

        #endregion Methods
    }
}