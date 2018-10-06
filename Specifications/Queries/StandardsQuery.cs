using DataAccess;
using LabDbContext;
using System.Linq;

namespace Specifications.Queries
{
    /// <summary>
    /// Query Object that returns multiple standard entries
    /// </summary>
    public class StandardsQuery : IQuery<Std, LabDbEntities>
    {
        #region Properties

        /// <summary>
        /// If true the results are ordered alphabetically by name
        /// </summary>
        public bool OrderResults { get; set; } = true;

        #endregion Properties

        #region Methods

        public IQueryable<Std> Execute(LabDbEntities context)
        {
            IQueryable<Std> query = context.Stds;

            if (OrderResults)
                query = query.OrderBy(std => std.Name);

            return query;
        }

        #endregion Methods
    }
}