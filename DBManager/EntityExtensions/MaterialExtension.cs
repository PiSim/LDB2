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
                if (entry.RecipeID == 0)
                    entry.RecipeID = entry.Recipe.ID;

                if (entry.ConstructionID == 0)
                    entry.ConstructionID = entry.Construction.ID;

                Material newEntry = new Material();

                entities.Materials.Add(newEntry);
                entities.Entry(newEntry).CurrentValues.SetValues(entry);
                entities.SaveChanges();

                entry.ID = newEntry.ID;
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
                                                        .Include(mat => mat.Construction.Aspect)
                                                        .Include(mat => mat.Construction.Project)
                                                        .Include(mat => mat.Construction.Type)
                                                        .Include(mat => mat.Recipe.Colour)
                                                        .First(mat => mat.ID == entry.ID);

                entities.Entry(entry).CurrentValues.SetValues(tempEntry);
            }
        }

        public static void SetConstruction(this Material entry,
                                            Construction constructionEntity)
        {
            // Sets the construction and ConstructionID properties of a given Material

            if (entry == null)
                throw new NotImplementedException();

            entry.Construction = constructionEntity;
            entry.ConstructionID = (constructionEntity == null) ? 0 : constructionEntity.ID;
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
