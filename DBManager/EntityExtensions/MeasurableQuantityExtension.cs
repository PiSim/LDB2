using System.Collections.Generic;
using System.Linq;

namespace LabDbContext.EntityExtensions
{
    public static class MeasurableQuantityExtension
    {
        #region Methods

        public static void Create(this MeasurableQuantity entry)
        {
            // Inserts a new MeasurableQuantity entry in the DB

            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.MeasurableQuantities.Add(entry);
                entities.SaveChanges();
            }
        }

        public static void Delete(this MeasurableQuantity entry)
        {
            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.Entry(entities
                        .MeasurableQuantities
                        .First(meq => meq.ID == entry.ID))
                        .State = System.Data.Entity.EntityState.Deleted;

                entities.SaveChanges();

                entry.ID = 0;
            }
        }

        public static IEnumerable<MeasurementUnit> GetMeasurementUnits(this MeasurableQuantity entry)
        {
            // Returns all MeasurementUnit for a given MeasurableQuantity

            if (entry == null)
                return new List<MeasurementUnit>();

            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.MeasurementUnits.Where(meu => meu.MeasurableQuantityID == entry.ID)
                                                .ToList();
            }
        }

        #endregion Methods
    }
}