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

        /// <summary>
        /// If set only the methods associated with the ExternalReport with the given ID are returned
        /// </summary>
        public int? ExternalReportID { get; set; }

        public bool IncludeObsolete { get; set; } = false;

        #endregion Properties

        #region Methods

        public override IQueryable<MethodVariant> Execute(LabDbEntities context)
        {
            context.Configuration.LazyLoadingEnabled = false;

            IQueryable<MethodVariant> query = context.MethodVariants;

            if (AsNoTracking)
                query.AsNoTracking();

            if (EagerLoadingEnabled)
                query = query.Include(mtdvar => mtdvar.Method.Property)
                            .Include(mtdvar => mtdvar.Method.Standard.Organization)
                            .Include(mtdvar => mtdvar.Method.SubMethods);

            if (!IncludeObsolete)
                query = query.Where(mtdvar => !mtdvar.IsOld);

            if (ExternalReportID != null)
                query = query.Where(mtdvar => mtdvar.ExternalReports.Any(exrep => exrep.ID == ExternalReportID));

            if (OrderResults)
                query = query.OrderBy(mtdvar => mtdvar.Method.Standard.Name)
                            .ThenBy(mtdvar => mtdvar.Name);

            return query;
        }

        #endregion Methods
    }
}