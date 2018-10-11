using DataAccess;
using LabDbContext;
using System.Data.Entity;
using System.Linq;

namespace Instruments.Queries
{
    /// <summary>
    /// Query object that returns multiple Instrument entities
    /// </summary>
    public class InstrumentsQuery : QueryBase<Instrument, LabDbEntities>
    {

        #region Methods

        public override IQueryable<Instrument> Execute(LabDbEntities context)
        {
            IQueryable<Instrument> query = context.Instruments;

            if (AsNoTracking)
                query = query.AsNoTracking();

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