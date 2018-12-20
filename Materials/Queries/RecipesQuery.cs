using DataAccess;
using LabDbContext;
using System.Data.Entity;
using System.Linq;

namespace Materials.Queries
{
    /// <summary>
    /// Query object that returns  multiple Recipe entities
    /// </summary>
    public class RecipesQuery : QueryBase<Recipe, LabDbEntities>
    {
        #region Properties

        /// <summary>
        /// Gets or Sets a ColorID to use as filter
        /// </summary>
        public int? ColorID { get; set; }

        #endregion Properties

        #region Methods

        public override IQueryable<Recipe> Execute(LabDbEntities context)
        {
            IQueryable<Recipe> query = context.Recipes;

            if (AsNoTracking)
                query = query.AsNoTracking();

            if (ColorID != null)
                query = query.Where(rec => rec.ColourID == ColorID);

            if (OrderResults)
                query = query.OrderBy(rec => rec.Code);

            return query;
        }

        #endregion Methods
    }
}