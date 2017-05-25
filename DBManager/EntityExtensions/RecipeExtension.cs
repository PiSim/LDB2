using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager.EntityExtensions
{
    public static class RecipeExtension
    {
        public static void Delete(this Recipe entry)
        {
            // Deletes a Recipe entry

            using (DBEntities entities = new DBEntities())
            {
                entities.Entry(entities.Recipes.First(rec => rec.ID == entry.ID))
                        .State = System.Data.Entity.EntityState.Deleted;

                entities.SaveChanges();
            }
        }
    }
}
