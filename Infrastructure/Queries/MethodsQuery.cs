using DataAccess;
using LabDbContext;
using System.Data.Entity;
using System.Linq;

namespace Infrastructure.Queries
{
    /// <summary>
    /// Query object that returns multiple Method entities
    /// </summary>
    public class MethodsQuery : QueryBase<Method, LabDbEntities>
    {
        #region Properties

        /// <summary>
        /// If false only the entities not flagged as IsOld are returned
        /// </summary>
        public bool IncludeObsolete { get; set; } = false;
        #endregion Properties

        #region Methods

        public override IQueryable<Method> Execute(LabDbEntities context)
        {
            IQueryable<Method> query = context.Methods;

            if (AsNoTracking)
                query = query.AsNoTracking();

            if (EagerLoadingEnabled)
                query = query.Include(mtd => mtd.Property)
                            .Include(mtd => mtd.Standard.Organization);

            if (!IncludeObsolete)
                query = query.Where(mtd => !mtd.IsOld);

            if (OrderResults)
                query = query.OrderBy(mtd => mtd.Standard.Name);

            return query;
        }

        #endregion Methods
    }
}