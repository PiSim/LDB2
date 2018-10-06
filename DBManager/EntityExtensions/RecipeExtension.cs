using System.Data.Entity.Migrations;
using System.Linq;

namespace LabDbContext.EntityExtensions
{
    public static class RecipeExtension
    {
        #region Methods

        public static void Create(this Recipe entry)
        {
            // Inserts a new Recipe entity in the DB

            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.Recipes.Add(entry);
                entities.SaveChanges();
            }
        }

        public static void Delete(this Recipe entry)
        {
            // Deletes a Recipe entry

            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.Entry(entities.Recipes.First(rec => rec.ID == entry.ID))
                        .State = System.Data.Entity.EntityState.Deleted;

                entities.SaveChanges();
            }
        }

        public static void Update(this Recipe entry)
        {
            // Updates a recipe entry

            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.Recipes.AddOrUpdate(entry);
                entities.SaveChanges();
            }
        }

        #endregion Methods
    }
}