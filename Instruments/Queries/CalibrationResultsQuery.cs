using DataAccess;
using LabDbContext;
using System.Data.Entity;
using System.Linq;

namespace Instruments.Queries
{
    /// <summary>
    /// Query object that returns multiple CalibrationResult entities
    /// </summary>
    public class CalibrationResultsQuery : QueryBase<CalibrationResult, LabDbEntities>
    {
        #region Methods

        public override IQueryable<CalibrationResult> Execute(LabDbEntities context)
        {
            IQueryable<CalibrationResult> query = context.CalibrationResults;

            if (LazyLoadingDisabled)
                context.Configuration.LazyLoadingEnabled = false;
            
            if (OrderResults)
                query = query.OrderBy(cres => cres.Description);

            if (AsNoTracking)
                query = query.AsNoTracking();

            return query;
        }

        #endregion Methods
    }
}