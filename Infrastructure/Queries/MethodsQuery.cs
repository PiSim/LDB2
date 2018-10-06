using DataAccess;
using LabDbContext;
using System.Linq;

namespace Infrastructure.Queries
{
    /// <summary>
    /// Query object that returns multiple Method entities
    /// </summary>
    public class MethodsQuery : IQuery<Method, LabDbEntities>
    {
        #region Properties

        /// <summary>
        /// If false only the entities not flagged as IsOld are returned
        /// </summary>
        public bool IncludeObsolete { get; set; } = false;

        /// <summary>
        /// If true the results are ordered by Std.Name
        /// </summary>
        public bool OrderResults { get; set; } = true;

        #endregion Properties

        #region Methods

        public IQueryable<Method> Execute(LabDbEntities context)
        {
            IQueryable<Method> query = context.Methods;

            if (!IncludeObsolete)
                query = query.Where(mtd => !mtd.IsOld);

            if (OrderResults)
                query = query.OrderBy(mtd => mtd.Standard.Name);

            return query;
        }

        #endregion Methods
    }
}