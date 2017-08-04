using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager.EntityExtensions
{
    public static class InstrumentTypeExtension
    {
        public static void AddMeasurableQuantityAssociation(this InstrumentType entry,
                                                            MeasurableQuantity quantity)
        {
            // Create a new association between an instrument Type and a quantity

            using (DBEntities entities = new DBEntities())
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

            using (DBEntities entities = new DBEntities())
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

            using (DBEntities entities = new DBEntities())
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

            using (DBEntities entities = new DBEntities())
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

            using (DBEntities entities = new DBEntities())
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
    }
}
