using DataAccess;
using LabDbContext;
using System.Linq;

namespace Infrastructure.Queries
{
    /// <summary>
    /// Query object that returns all Material enitites
    /// </summary>
    public class MaterialsQuery : IQuery<Material, LabDbEntities>
    {
        #region Properties

        /// <summary>
        /// If True the results are ordered by TypeCode then by LineCode then by AspectCode then by RecipeCode
        /// </summary>
        public bool OrderResults { get; set; } = true;

        #endregion Properties

        #region Methods

        public IQueryable<Material> Execute(LabDbEntities context)
        {
            IQueryable<Material> query = context.Materials;

            if (OrderResults)
                query = query.OrderBy(mat => mat.MaterialType.Code)
                            .ThenBy(mat => mat.MaterialLine.Code)
                            .ThenBy(mat => mat.Aspect.Code)
                            .ThenBy(mat => mat.Recipe.Code);

            return query;
        }

        #endregion Methods
    }
}