using DataAccess;
using LabDbContext;
using System.Data.Entity;
using System.Linq;

namespace Infrastructure.Queries
{
    /// <summary>
    /// Query object that returns multiple ExternalReport entities
    /// </summary>
    public class ExternalReportsQuery : QueryBase<ExternalReport, LabDbEntities>
    {
        #region Methods

        public override IQueryable<ExternalReport> Execute(LabDbEntities context)
        {
            if (LazyLoadingDisabled)
                context.Configuration.LazyLoadingEnabled = false;

            IQueryable<ExternalReport> query = context.ExternalReports;

            if (AsNoTracking)
                query = query.AsNoTracking();

            if (EagerLoadingEnabled)
                query.Include(exrep => exrep.ExternalLab);

            if (OrderResults)
                query = query.OrderByDescending(exrep => exrep.Year)
                            .ThenByDescending(exrep => exrep.Number);

            return query;
        }

        #endregion Methods
    }
}