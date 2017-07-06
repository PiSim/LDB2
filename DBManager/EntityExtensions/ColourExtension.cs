using DBManager;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager.EntityExtensions
{
    public static class ColourExtension
    {
        public static void Create(this Colour entry)
        {
            // Inserts a Colour entry in the DB

            using (DBEntities entities = new DBEntities())
            {
                entities.Colours.Add(entry);
                entities.SaveChanges();
            }
        }

        public static void Delete(this Colour entry)
        {
            // Deletes a Colour entity

            using (DBEntities entities = new DBEntities())
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

            using (DBEntities entities = new DBEntities())
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

        public static void Update(this Colour entry)
        {
            // Updates a colour entry

            using (DBEntities entities = new DBEntities())
            {
                entities.Colours.AddOrUpdate(entry);
                entities.SaveChanges();
            }
        }
    }
}
