using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace LabDbContext.EntityExtensions
{
    public static class InstrumentTypeExtension
    {
        #region Methods

        public static void AddMeasurableQuantityAssociation(this InstrumentType entry,
                                                            MeasurableQuantity quantity)
        {
            // Create a new association between an instrument Type and a quantity

            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.InstrumentTypes
                        .First(ist => ist.ID == entry.ID)
                        .MeasurableQuantities
                        .Add(entities
                        .MeasurableQuantities
                        .First(meq => meq.ID == quantity.ID));

                entities.SaveChanges();
            }
        }

        public static void Create(this InstrumentType entry)
        {
            // Inserts a new InstrumentType in the DB

            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.InstrumentTypes.Add(entry);
                entities.SaveChanges();
            }
        }

        public static IEnumerable<MeasurableQuantity> GetAssociatedMeasurableQuantities(this InstrumentType entry)
        {
            // Returns all MeasurableQuantity entities related to the instrument type

            if (entry == null)
                return null;

            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.MeasurableQuantities.Include(meq => meq.UnitsOfMeasurement)
                                                    .Where(meq => meq.InstrumentTypes
                                                    .Any(mit => mit.ID == entry.ID))
                                                    .ToList();
            }
        }

        public static IEnumerable<MeasurableQuantity> GetUnassociatedMeasurableQuantities(this InstrumentType entry)
        {
            // Returns all MeasurableQuantity entities NOT related to the instrument type

            if (entry == null)
                return null;

            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.MeasurableQuantities.Where(meq => !meq.InstrumentTypes
                                                    .Any(mit => mit.ID == entry.ID))
                                                    .ToList();
            }
        }

        public static void RemoveMeasurableQuantityAssociation(this InstrumentType entry,
                                                                MeasurableQuantity quantity)
        {
            // Removes an association between an instrument Type and a quantity

            using (LabDbEntities entities = new LabDbEntities())
            {
                MeasurableQuantity toBeRemoved = entities.MeasurableQuantities
                                                        .First(meq => meq.ID == quantity.ID);

                entities.InstrumentTypes
                        .First(ist => ist.ID == entry.ID)
                        .MeasurableQuantities
                        .Remove(toBeRemoved);

                entities.SaveChanges();
            }
        }

        #endregion Methods
    }
}