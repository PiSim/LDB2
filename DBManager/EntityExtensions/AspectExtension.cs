using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager
{
    public partial class Aspect
    {
        private IEnumerable<Batch> _batches;

        public IEnumerable<Batch> Batches => _batches;

        public IEnumerable<Batch> GetBatches()
        {
            // Returns a list of the batches for an Aspect and stores it in the instance

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                _batches = entities.Batches
                                    .Include(btc => btc.Material.Aspect)
                                    .Include(btc => btc.Material.ExternalConstruction)
                                    .Include(btc => btc.Material.MaterialLine)
                                    .Include(btc => btc.Material.MaterialType)
                                    .Include(btc => btc.Material.Recipe.Colour)
                                    .Where(btc => btc.Material.AspectID == ID)
                                    .ToList();

                return _batches;
            }
        }
    }

    public static class AspectExtension
    {
        public static void Create(this Aspect entry)
        {
            if (entry == null)
                throw new NullReferenceException();

            using (DBEntities entities = new DBEntities())
            {
                entities.Aspects.Add(entry);
                entities.SaveChanges();
            }
        }

        public static void Delete(this Aspect entry)
        {
            // Deletes an aspect entity

            if (entry == null)
                throw new NullReferenceException();

            using (DBEntities entities = new DBEntities())
            {
                entities.Entry(entities.Aspects.First(asp => asp.ID == entry.ID))
                        .State = EntityState.Deleted;
                entities.SaveChanges();
            }
        }

        public static void Load(this Aspect entry)
        {
            // Loads the values of an Aspect entry from the DB

            if (entry == null)
                return;

            using (DBEntities entities = new DBEntities())
            {
                entities.Aspects.Attach(entry);

                Aspect tempEntry = entities.Aspects.First(asp => asp.ID == entry.ID);
                entities.Entry(entry).CurrentValues.SetValues(tempEntry);
            }
        }

        public static void Update(this Aspect entry)
        {
            // Updates a given Aspect entry

            if (entry == null)
                throw new NullReferenceException();

            using (DBEntities entities = new DBEntities())
            {
                Aspect tempEntry = entities.Aspects.First(asp => asp.ID == entry.ID);

                entities.Entry(tempEntry).CurrentValues.SetValues(entry);
                entities.SaveChanges();
            }
        }
    }
}
