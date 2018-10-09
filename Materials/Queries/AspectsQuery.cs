using DataAccess;
using LabDbContext;
using System.Data.Entity;
using System.Linq;

namespace Materials.Queries
{
    /// <summary>
    /// Query Object that returns all Aspect Entities
    /// </summary>
    public class AspectsQuery : QueryBase<Aspect, LabDbEntities>
    {
        #region Methods

        public override IQueryable<Aspect> Execute(LabDbEntities context)
        {
            IQueryable<Aspect> query = context.Aspects;

            if (AsNoTracking)
                query = query.AsNoTracking();

            if (OrderResults)
                query = query.OrderByDescending(asp => asp.Code);


            return query;
        }

        #endregion Methods
    }
}