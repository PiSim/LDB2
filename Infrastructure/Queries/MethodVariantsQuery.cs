using DataAccess;
using LabDbContext;
using System.Data.Entity;
using System.Linq;

namespace Infrastructure.Queries
{
    /// <summary>
    /// Object for querying the context for MethodVariant Entities.
    /// IncludeObsolete (default False) : If true the objects flagged as IsOld are included in the search
    /// SortResults (default True) : if True the results are sorted by Method Name then by Variant Name
    /// </summary>
    public class MethodVariantsQuery : QueryBase<MethodVariant, LabDbEntities>
    {
        #region Properties

        public bool IncludeObsolete { get; set; } = false;

        #endregion Properties

        #region Methods

        public override IQueryable<MethodVariant> Execute(LabDbEntities context)
        {
            context.Configuration.LazyLoadingEnabled = false;

            IQueryable<MethodVariant> query = context.MethodVariants;
            
            if (EagerLoadingEnabled)
                query = query.Include(mtdvar => mtdvar.Method.Property)
                            .Include(mtdvar => mtdvar.Method.Standard.Organization);

            if (!IncludeObsolete)
                query = query.Where(mtdvar => !mtdvar.IsOld);

            if (OrderResults)
                query = query.OrderBy(mtdvar => mtdvar.Method.Standard.Name)
                            .ThenBy(mtdvar => mtdvar.Name);

            if (AsNoTracking)
                query.AsNoTracking();

            return query;
        }

        #endregion Methods
    }
}