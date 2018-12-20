using DataAccessCore;
using LInst;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Instruments.Queries
{
    /// <summary>
    /// Query object that returns all the Instrument instances marked as reference for a given CalibrationReport
    /// </summary>
    public class ReferenceInstrumentsQuery : QueryBase<Instrument, LInstContext>
    {
        #region Fields

        private CalibrationReport _reportInstance;

        #endregion Fields

        #region Constructors

        public ReferenceInstrumentsQuery(CalibrationReport reportInstance)
        {
            _reportInstance = reportInstance;
        }

        #endregion Constructors

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