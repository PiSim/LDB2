using DataAccess;
using LabDbContext;
using System.Data.Entity;
using System.Linq;

namespace Instruments.Queries
{
    /// <summary>
    /// Query object that returns multiple CalibrationReport entities
    /// </summary>
    public class CalibrationReportsQuery : IQuery<CalibrationReport, LabDbEntities>
    {
        #region Properties

        /// <summary>
        /// If true the query is run AsNoTracking
        /// </summary>
        public bool AsNoTracking { get; set; } = true;

        /// <summary>
        /// If true the relevant subentities are eagerly loaded
        /// </summary>
        public bool EagerLoadingEnabled { get; set; } = true;

        /// <summary>
        /// If true the results are ordered by year descending then number descending
        /// </summary>
        public bool OrderResults { get; set; }

        #endregion Properties

        #region Methods

        public IQueryable<CalibrationReport> Execute(LabDbEntities context)
        {
            IQueryable<CalibrationReport> query = context.CalibrationReports;

            if (EagerLoadingEnabled)
                query = query.Include(crep => crep.Instrument)
                            .Include(crep => crep.CalibrationResult)
                            .Include(crep => crep.Laboratory)
                            .Include(crep => crep.Tech);

            if (OrderResults)
                query = query.OrderByDescending(crep => crep.Year)
                            .ThenByDescending(crep => crep.Number);

            if (AsNoTracking)
                query = query.AsNoTracking();

            return query;
        }

        #endregion Methods
    }
}