using LInst;
using DataAccessCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Instruments.Queries
{
    /// <summary>
    /// Query Object that returns a single CalibrationReport object
    /// </summary>
    public class CalibrationReportQuery : ScalarBase<CalibrationReport, LInstContext>
    {
        public int? ID { get; set; }

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
    }
}
