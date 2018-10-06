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
    /// Query object that returns multiple Requirement entities
    /// </summary>
    public class RequirementsQuery : QueryBase<Requirement, LabDbEntities>
    {
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
    }
}
