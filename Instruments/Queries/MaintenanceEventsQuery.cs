using DataAccess;
using LabDbContext;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instruments.Queries
{
    /// <summary>
    /// Query object that returns multiple InstrumentMaintenanceEvents allowing filter by InstrumentID
    /// </summary>
    public class MaintenanceEventsQuery : QueryBase<InstrumentMaintenanceEvent, LabDbEntities>
    {
        /// <summary>
        /// InstrumentID to use as filter in the query
        /// </summary>
        public int? InstrumentID { get; set; }
        

        public override IQueryable<InstrumentMaintenanceEvent> Execute(LabDbEntities context)
        {
            IQueryable<InstrumentMaintenanceEvent> query = context.InstrumentMaintenanceEvents;

            if (LazyLoadingDisabled)
                context.Configuration.LazyLoadingEnabled = false;

            if (AsNoTracking)
                query = query.AsNoTracking();

            if (InstrumentID != null)
                query = query.Where(ime => ime.InstrumentID == InstrumentID);

            return query;
        }
    }
}
