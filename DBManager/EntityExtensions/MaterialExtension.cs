using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager.EntityExtensions
{
    public static class MaterialExtension
    {

        public static void Create(this Material entry)
        {
            // Inserts a new Material entry
            
            using (DBEntities entities = new DBEntities())
            {
                entities.Materials.Add(entry);
                entities.SaveChanges();
            }
        }

        public static IEnumerable<Batch> GetBatches(this Material entry)
        {
            // Returns all the batches for a given material

            if (entry == null)
                return null;

            using (DBEntities entitites = new DBEntities())
            {
                entitites.Configuration.LazyLoadingEnabled = false;

                return entitites.Batches.Include(btc => btc.Material.Aspect)
                                        .Include(btc => btc.Material.MaterialLine)
                                        .Include(btc => btc.Material.MaterialType)
                                        .Include(btc => btc.Material.Recipe.Colour)
                                        .Where(btc => btc.MaterialID == entry.ID)
                                        .OrderBy(btc => btc.Number)
                                        .ToList();
            }
        }

        public static void Delete(this Material entry)
        {
            // Deletes a Material entry

            if (entry == null)
                throw new NullReferenceException();

            using (DBEntities entities = new DBEntities())
            {
                Material tempEntry = entities.Materials.First(mat => mat.ID == entry.ID);
                entities.Entry(tempEntry).State = EntityState.Deleted;
                entities.SaveChanges();
            }
        }

        public static void Load(this Material entry)
        {
            // Loads DB values into a given material entry

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                entities.Materials.Attach(entry);

                Material tempEntry = entities.Materials.Include(mat => mat.Batches)
                                                        .Include(mat => mat.Aspect)
                                                        .Include(mat => mat.MaterialLine)
                                                        .Include(mat => mat.MaterialType)
                                                        .Include(mat => mat.Project)
                                                        .Include(mat => mat.Recipe.Colour)
                                                        .First(mat => mat.ID == entry.ID);

                entities.Entry(entry).CurrentValues.SetValues(tempEntry);
            }
        }

        public static void SetRecipe(this Material entry,
                                        Recipe recipeEntity)
        {
            // Sets the Recipe and RecipeID properties of a given Material

            if (entry == null)
                throw new NotImplementedException();

            entry.Recipe = recipeEntity;
            entry.RecipeID = (recipeEntity == null) ? 0 : recipeEntity.ID;
        }

        public static void Update(this Material entry)
        {
            // Updates the DB values of a Material entity

            using (DBEntities entities = new DBEntities())
            {
                Material tempEntry = entities.Materials.First(mat => mat.ID == entry.ID);
                entities.Entry(tempEntry).CurrentValues.SetValues(entry);

                entities.SaveChanges();
            }
        }

    }
}
