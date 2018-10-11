using DataAccess;
using LabDbContext;
using System.Data.Entity;
using System.Linq;

namespace Specifications.Queries
{
    public class SpecificationsQuery : QueryBase<Specification, LabDbEntities>
    {
        #region Methods

        public override IQueryable<Specification> Execute(LabDbEntities context)
        {
            IQueryable<Specification> query = context.Specifications;

            if (EagerLoadingEnabled)
                query = query.Include(spec => spec.Standard.Organization);

            if (AsNoTracking)
                query = query.AsNoTracking();

            if (OrderResults)
                query = query.OrderBy(spec => spec.Standard.Organization.Name)
                            .ThenBy(spec => spec.Standard.Name);

            return query;
        }

        #endregion Methods
    }
}