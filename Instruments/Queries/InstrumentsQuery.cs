using DataAccessCore;
using LInst;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Instruments.Queries
{
    /// <summary>
    /// Query object that returns multiple Instrument entities
    /// </summary>
    public class InstrumentsQuery : QueryBase<Instrument, LInstContext>
    {
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

            return query;
        }

        #endregion Methods
    }
}