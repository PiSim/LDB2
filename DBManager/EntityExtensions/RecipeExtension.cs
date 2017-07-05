using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager.EntityExtensions
{
    public static class RecipeExtension
    {
        public static void Create(this Recipe entry)
        {
            // Inserts a new Recipe entity in the DB

            using (DBEntities entities = new DBEntities())
            {
                entities.Recipes.Add(entry);
                entities.SaveChanges();
            }
        }

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

        public static void Update(this Recipe entry)
        {
            // Updates a recipe entry

            using (DBEntities entities = new DBEntities())
            {
                entities.Recipes.AddOrUpdate(entry);
                entities.SaveChanges();
            }
        }
    }
}
