using DataAccess;
using LabDbContext;
using System.Data.Entity;
using System.Linq;

namespace Materials.Queries
{
    /// <summary>
    /// Query objects that returns All ExternalConstruction entities
    /// OrderResults (def. true) : if true the results are ordered by Oem then by Name
    /// </summary>
    public class ExternalConstructionsQuery : IQuery<ExternalConstruction, LabDbEntities>
    {
        #region Properties

        public bool OrderResults { get; set; } = true;

        #endregion Properties

        #region Methods

        public IQueryable<ExternalConstruction> Execute(LabDbEntities context)
        {
            IQueryable<ExternalConstruction> query = context.ExternalConstructions.Include(exc => exc.Oem);

            if (OrderResults)
                query = query.OrderBy(exc => exc.Oem.Name)
                            .ThenBy(exc => exc.Name);

            return query;
        }

        #endregion Methods
    }
}