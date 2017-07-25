using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager.EntityExtensions
{
    public static class InstrumentTypeExtension
    {
        public static IEnumerable<MeasurableQuantity> GetAssociatedMeasurableQuantities(this InstrumentType entry)
        {
            // Returns all MeasurableQuantity entities related to the instrument type

            if (entry == null)
                return null;

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.MeasurableQuantities.Where(meq => meq.InstrumentTypes
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
    }
}
