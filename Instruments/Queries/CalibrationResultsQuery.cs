using DataAccessCore;
using LInst;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Instruments.Queries
{
    /// <summary>
    /// Query object that returns multiple CalibrationResult entities
    /// </summary>
    public class CalibrationResultsQuery : QueryBase<CalibrationResult, LInstContext>
    {
        #region Methods

        public override IQueryable<CalibrationResult> Execute(LInstContext context)
        {
            IQueryable<CalibrationResult> query = context.CalibrationResults;

            if (OrderResults)
                query = query.OrderBy(cres => cres.Description);

            if (AsNoTracking)
                query = query.AsNoTracking();

            return query;
        }

        #endregion Methods
    }
}