using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace LabDbContext.EntityExtensions
{
    public static class ColourExtension
    {
        #region Methods

        public static void Create(this Colour entry)
        {
            // Inserts a Colour entry in the DB

            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.Colours.Add(entry);
                entities.SaveChanges();
            }
        }

        public static void Delete(this Colour entry)
        {
            // Deletes a Colour entity

            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.Entry(entities
                        .Colours
                        .First(col => col.ID == entry.ID))
                        .State = EntityState.Deleted;

                entities.SaveChanges();
                entry.ID = 0;
            }
        }

        public static IEnumerable<Batch> GetBatches(this Colour entry)
        {
            // Returns all Batches for a given colour

            if (entry == null)
                return null;

            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Batches.Include(bat => bat.Material.Aspect)
                                        .Include(bat => bat.Material.MaterialLine)
                                        .Include(bat => bat.Material.MaterialType)
                                        .Include(bat => bat.Material.Recipe.Colour)
                                        .Where(bat => bat.Material.Recipe.ColourID == entry.ID)
                                        .ToList();
            }
        }

        public static IEnumerable<Recipe> GetRecipes(this Colour entry)
        {
            if (entry == null)
                return new List<Recipe>();

            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Recipes
                                .Where(rec => rec.ColourID == entry.ID)
                                .ToList();
            }
        }

        public static void Update(this Colour entry)
        {
            // Updates a colour entry

            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.Colours.AddOrUpdate(entry);
                entities.SaveChanges();
            }
        }

        #endregion Methods
    }
}