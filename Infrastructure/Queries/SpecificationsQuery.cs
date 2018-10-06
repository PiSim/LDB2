using DataAccess;
using LabDbContext;
using System.Data.Entity;
using System.Linq;

namespace Specifications.Queries
{
    public class SpecificationsQuery : IQuery<Specification, LabDbEntities>
    {
        #region Properties

        /// <summary>
        /// If true the query is run AsNoTracking
        /// </summary>
        public bool AsNoTracking { get; set; } = true;

        /// <summary>
        /// If true the relevant subentities are eagerly loaded
        /// </summary>
        public bool EagerLoadingEnabled { get; set; } = true;

        /// <summary>
        /// If true the results are ordered by code
        /// </summary>
        public bool OrderResults { get; set; }

        #endregion Properties

        #region Methods

        public IQueryable<Specification> Execute(LabDbEntities context)
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