using DataAccess;
using LabDbContext;
using System.Linq;

namespace Infrastructure.Queries
{
    /// <summary>
    /// Query object that returns all Properties.
    /// </summary>
    public class PropertiesQuery : IQuery<Property, LabDbEntities>
    {
        #region Properties

        /// <summary>
        /// If true the results are sorted by name
        /// </summary>
        public bool OrderResults { get; set; } = true;

        #endregion Properties

        #region Methods

        public IQueryable<Property> Execute(LabDbEntities context)
        {
            IQueryable<Property> query = context.Properties;

            if (OrderResults)
                query = query.OrderBy(pro => pro.Name);

            return query;
        }

        #endregion Methods
    }
}