using DataAccess;
using LabDbContext;
using System.Data.Entity;
using System.Linq;

namespace Infrastructure.Queries
{
    /// <summary>
    /// Query object that returns multiple ControlPlan entities
    /// </summary>
    public class ControlPlansQuery : QueryBase<ControlPlan, LabDbEntities>
    {
        #region Properties

        /// <summary>
        /// Get or set a SpecificationID to filter by
        /// </summary>
        public int? SpecificationID { get; set; }

        #endregion Properties

        #region Methods

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

        #endregion Methods
    }
}