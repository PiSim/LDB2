using DataAccess;
using LabDbContext;
using System.Data.Entity;
using System.Linq;

namespace Infrastructure.Queries
{
    /// <summary>
    /// Query object that returns multiple Requirement entities
    /// </summary>
    public class RequirementsQuery : QueryBase<Requirement, LabDbEntities>
    {
        #region Methods

        public override IQueryable<Requirement> Execute(LabDbEntities context)
        {
            IQueryable<Requirement> query = context.Requirements;

            if (AsNoTracking)
                query = query.AsNoTracking();

            if (EagerLoadingEnabled)
                query = query.Include(req => req.MethodVariant.Method.Standard.Organization)
                            .Include(req => req.MethodVariant.Method.Property);

            if (OrderResults)
                query = query.OrderBy(req => req.Position);

            return query;
        }

        #endregion Methods
    }
}