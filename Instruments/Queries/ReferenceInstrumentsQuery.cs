using LInst;
using DataAccessCore;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Instruments.Queries
{
    /// <summary>
    /// Query object that returns all the Instrument instances marked as reference for a given CalibrationReport
    /// </summary>
    public class ReferenceInstrumentsQuery : QueryBase<Instrument, LInstContext>
    {
        private CalibrationReport _reportInstance;

        public ReferenceInstrumentsQuery(CalibrationReport reportInstance)
        {
            _reportInstance = reportInstance;
        }

        #region Methods

        public override IQueryable<Instrument> Execute(LInstContext context)
        {
            IQueryable<Instrument> query = context.Instruments;

            if (AsNoTracking)
                query = query.AsNoTracking();

            if (EagerLoadingEnabled)
                query = query.Include(inst => inst.InstrumentType)
                    .Include(inst => inst.Manufacturer)
                    .Include(inst => inst.UtilizationArea);

            if (OrderResults)
                query = query.OrderBy(inst => inst.Code);

            return query.Where(ins => ins.CalibrationsAsReference
                        .Any(crr => crr.CalibrationReportID == _reportInstance.ID));
        }

        #endregion Methods
    }
}