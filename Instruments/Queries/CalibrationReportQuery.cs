using DataAccessCore;
using LInst;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Instruments.Queries
{
    /// <summary>
    /// Query Object that returns a single CalibrationReport object
    /// </summary>
    public class CalibrationReportQuery : ScalarBase<CalibrationReport, LInstContext>
    {
        #region Properties

        public int? ID { get; set; }

        #endregion Properties

        #region Methods

        public override CalibrationReport Execute(LInstContext context)
        {
            IQueryable<CalibrationReport> query = context.CalibrationReports;

            if (EagerLoadingEnabled)
                query = query.Include(crep => crep.Instrument)
                            .Include(crep => crep.CalibrationResult)
                            .Include(crep => crep.Laboratory)
                            .Include(crep => crep.Tech);

            if (AsNoTracking)
                query = query.AsNoTracking();

            return query.FirstOrDefault(calrep => calrep.ID == ID);
        }

        #endregion Methods
    }
}