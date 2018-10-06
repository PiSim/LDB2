using DataAccess;
using LabDbContext;
using System.Data.Entity;
using System.Linq;

namespace Instruments.Queries
{
    /// <summary>
    /// Query object that returns multiple Instrument entities
    /// </summary>
    public class InstrumentsQuery : IQuery<Instrument, LabDbEntities>
    {
        #region Properties

        /// <summary>
        /// If true the query is returned AsNoTracking
        /// </summary>
        public bool AsNoTracking { get; set; } = true;

        /// <summary>
        /// If true the relevant subentities are eagerly loaded
        /// </summary>
        public bool EagerLoadingEnabled { get; set; } = true;

        /// <summary>
        /// If true the results are ordered by code
        /// </summary>
        public bool OrderResults { get; set; } = true;

        #endregion Properties

        #region Methods

        public IQueryable<Instrument> Execute(LabDbEntities context)
        {
            IQueryable<Instrument> query = context.Instruments;

            if (EagerLoadingEnabled)
                query = query.Include(inst => inst.InstrumentType)
                    .Include(inst => inst.Manufacturer)
                    .Include(inst => inst.InstrumentUtilizationArea);

            if (OrderResults)
                query = query.OrderBy(inst => inst.Code);

            return query;
        }

        #endregion Methods
    }
}