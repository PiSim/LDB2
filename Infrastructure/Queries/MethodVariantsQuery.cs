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
    public class MethodVariantsQuery : IQuery<MethodVariant, LabDbEntities>
    {
        #region Properties

        public bool AsNoTracking { get; set; } = true;

        public bool IncludeObsolete { get; set; } = false;

        public bool OrderResults { get; set; } = true;

        #endregion Properties

        #region Methods

        public IQueryable<MethodVariant> Execute(LabDbEntities context)
        {
            context.Configuration.LazyLoadingEnabled = false;

            IQueryable<MethodVariant> query = context.MethodVariants.Include(mtdvar => mtdvar.Method.Property)
                                                                    .Include(mtdvar => mtdvar.Method.Standard.Organization);

            if (!IncludeObsolete)
                query = query.Where(mtdvar => !mtdvar.IsOld);

            if (OrderResults)
                query = query.OrderBy(mtdvar => mtdvar.Method.Name)
                            .ThenBy(mtdvar => mtdvar.Name);

            if (AsNoTracking)
                query.AsNoTracking();

            return query;
        }

        #endregion Methods
    }
}