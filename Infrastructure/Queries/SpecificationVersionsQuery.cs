using DataAccess;
using LabDbContext;
using System.Data.Entity;
using System.Linq;

namespace Infrastructure.Queries
{
    public class SpecificationVersionsQuery : QueryBase<SpecificationVersion, LabDbEntities>
    {
        #region Properties

        public int? SpecificationID { get; set; }

        #endregion Properties

        #region Methods

        public override IQueryable<SpecificationVersion> Execute(LabDbEntities context)
        {
            IQueryable<SpecificationVersion> query = context.SpecificationVersions;

            if (LazyLoadingDisabled)
                context.Configuration.LazyLoadingEnabled = false;

            if (AsNoTracking)
                query = query.AsNoTracking();

            if (SpecificationID != null)
                query = query.Where(specv => specv.SpecificationID == SpecificationID);

            if (OrderResults)
                query = query.OrderBy(specv => specv.Name);

            return query;
        }

        #endregion Methods
    }
}