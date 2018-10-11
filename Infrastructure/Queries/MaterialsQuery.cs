using DataAccess;
using LabDbContext;
using System.Data.Entity;
using System.Linq;

namespace Infrastructure.Queries
{
    /// <summary>
    /// Query object that returns all Material enitites
    /// </summary>
    public class MaterialsQuery : QueryBase<Material, LabDbEntities>
    {
        #region Methods

        public override IQueryable<Material> Execute(LabDbEntities context)
        {
            IQueryable<Material> query = context.Materials;

            if (AsNoTracking)
                query = query.AsNoTracking();

            if (EagerLoadingEnabled)
                query = query.Include(mat => mat.Aspect)
                            .Include(mat => mat.MaterialLine)
                            .Include(mat => mat.MaterialType)
                            .Include(mat => mat.Recipe.Colour);

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