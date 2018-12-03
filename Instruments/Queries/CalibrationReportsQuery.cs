using DataAccessCore;
using LInst;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Instruments.Queries
{
    /// <summary>
    /// Query object that returns multiple CalibrationReport entities
    /// </summary>
    public class CalibrationReportsQuery : QueryBase<CalibrationReport, LInstContext>
    {

        #region Methods

        public override IQueryable<CalibrationReport> Execute(LInstContext context)
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