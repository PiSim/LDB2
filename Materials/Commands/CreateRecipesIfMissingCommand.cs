using DataAccess;
using LabDbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Materials.Commands
{
    /// <summary>
    /// Command objects that iterates through an IEnumerable of Recipe entries,
    /// checking if an entry with the same code exists in the database and inserting it if it doesn't
    /// </summary>
    public class CreateRecipesIfMissingCommand : ICommand<LabDbEntities>
    {
        private IEnumerable<Recipe> _recipeList;

        public CreateRecipesIfMissingCommand(IEnumerable<Recipe> recipeList)
        {
            _recipeList = recipeList;
        }

        public void Execute(LabDbEntities context)
        {
            foreach (Recipe newRec in _recipeList)
                if (!context.Recipes.Any(rec => rec.Code == newRec.Code))
                    context.Recipes.Add(new Recipe() { Code = newRec.Code });

            context.SaveChanges();
        }
    }
}
