using DataAccess;
using LabDbContext;
using System.Linq;

namespace Materials.Queries
{
    /// <summary>
    /// Query object that returns all ExternalConstruction entities
    /// OrderResults (def. true) : If True the results are sorted by OEM then by name
    /// </summary>
    public class ConstructionsQuery : IQuery<ExternalConstruction, LabDbEntities>
    {
        #region Properties

        public bool OrderResults { get; set; } = true;

        #endregion Properties

        #region Methods

        public IQueryable<ExternalConstruction> Execute(LabDbEntities context)
        {
            IQueryable<ExternalConstruction> query = context.ExternalConstructions;

            if (OrderResults)
                query = query.OrderBy(con => con.Oem.Name)
                            .ThenBy(con => con.Name);

            return query;
        }

        #endregion Methods
    }
}