using DataAccess;
using LabDbContext;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Queries
{
    /// <summary>
    /// Query object that returns multiple ControlPlan entities
    /// </summary>
    public class ControlPlansQuery : QueryBase<ControlPlan, LabDbEntities>
    {
        /// <summary>
        /// Get or set a SpecificationID to filter by
        /// </summary>
        public int? SpecificationID { get; set; }

        public override IQueryable<ControlPlan> Execute(LabDbEntities context)
        {
            IQueryable<ControlPlan> query = context.ControlPlans;

            if (LazyLoadingDisabled)
                context.Configuration.LazyLoadingEnabled = false;

            if (AsNoTracking)
                query = query.AsNoTracking();

            if (SpecificationID != null)
                query = query.Where(cpl => cpl.SpecificationID == SpecificationID);

            if (OrderResults)
                query = query.OrderBy(cpl => cpl.Name);

            return query;
        }
    }
}
