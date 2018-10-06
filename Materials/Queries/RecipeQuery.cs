using DataAccess;
using LabDbContext;
using System.Linq;

namespace Materials.Queries
{
    /// <summary>
    /// Query Object that returns the First recipe entry with a given Code or ID
    /// If both are given, the ID is given precedence
    /// RecipeCode : the Code to search for
    /// RecipeID : the ID to search for
    /// </summary>
    public class RecipeQuery : IScalar<Recipe, LabDbEntities>
    {
        #region Properties

        public string RecipeCode { get; set; }

        public int? RecipeID { get; set; } = null;

        #endregion Properties

        #region Methods

        public Recipe Execute(LabDbEntities context)
        {
            if (RecipeID != null)
                return context.Recipes.FirstOrDefault(rec => rec.ID == RecipeID);

            return context.Recipes.FirstOrDefault(rec => rec.Code == RecipeCode);
        }

        #endregion Methods
    }
}