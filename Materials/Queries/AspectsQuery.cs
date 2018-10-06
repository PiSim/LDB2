using DataAccess;
using LabDbContext;
using System.Linq;

namespace Materials.Queries
{
    /// <summary>
    /// Query Object that returns all Aspect Entities
    /// </summary>
    public class AspectsQuery : IQuery<Aspect, LabDbEntities>
    {
        #region Properties

        /// <summary>
        /// If true the results are ordered by code descending
        /// </summary>
        public bool OrderResults { get; set; } = true;

        #endregion Properties

        #region Methods

        public IQueryable<Aspect> Execute(LabDbEntities context)
        {
            IQueryable<Aspect> query = context.Aspects;

            if (OrderResults)
                query = query.OrderByDescending(asp => asp.Code);

            return query;
        }

        #endregion Methods
    }
}