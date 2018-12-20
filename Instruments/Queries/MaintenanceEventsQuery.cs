using DataAccessCore;
using LInst;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Instruments.Queries
{
    /// <summary>
    /// Query object that returns multiple InstrumentMaintenanceEvents allowing filter by InstrumentID
    /// </summary>
    public class MaintenanceEventsQuery : QueryBase<InstrumentMaintenanceEvent, LInstContext>
    {
        #region Properties

        /// <summary>
        /// InstrumentID to use as filter in the query
        /// </summary>
        public int? InstrumentID { get; set; }

        #endregion Properties

        #region Methods

        public override IQueryable<InstrumentMaintenanceEvent> Execute(LInstContext context)
        {
            IQueryable<InstrumentMaintenanceEvent> query = context.InstrumentMaintenanceEvents;

            if (AsNoTracking)
                query = query.AsNoTracking();

            if (InstrumentID != null)
                query = query.Where(ime => ime.InstrumentID == InstrumentID);

            return query;
        }

        #endregion Methods
    }
}