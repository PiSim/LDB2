using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager
{
    public static class DataService
    {
        // Repository for Common DB-Access methods

        public static IEnumerable<Currency> GetCurrencies()
        {
            // Returns all currency entities

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Currencies.Where(cur => true)
                                            .ToList();
            }
        }

        public static IEnumerable<Batch> GetBatchesForExternalConstruction(ExternalConstruction target,
                                                                    bool lazyLoadingEnabled = false)
        {
            using (var entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = lazyLoadingEnabled;
                return entities.Batches.Where(btc => btc.Material.Construction.ExternalConstruction.ID == target.ID)
                                        .Include(btc => btc.Material.Construction.Type)
                                        .Include(btc => btc.Material.Construction.Aspect)
                                        .Include(btc => btc.Material.Recipe.Colour)
                                        .ToList();
            }
        }

        public static IEnumerable<Construction> GetConstructionsWithoutExternal()
        {
            using (var entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;
                return entities.Constructions.Where(cns => cns.ExternalConstruction == null)
                                            .Include(cns => cns.Aspect)
                                            .Include(cns => cns.Type)
                                            .ToList();
            }
        }

        public static IEnumerable<Construction> GetConstructionsWithoutProject()
        {
            using (var entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;
                return entities.Constructions.Where(cns => cns.Project == null)
                                            .Include(cns => cns.Aspect)
                                            .Include(cns => cns.ExternalConstruction)
                                            .Include(cns => cns.Type)
                                            .ToList();
            }
        }

        public static IEnumerable<InstrumentType> GetInstrumentTypes()
        {
            using (var entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.InstrumentTypes
                                .Where(insty => true)
                                .OrderBy(insty => insty.Name)
                                .ToList();
            }
        }

        public static IEnumerable<Property> GetProperties()
        {
            // Returns all Property entities

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Properties.Where(prp => true)
                                            .ToList();
            }
        }
    }
}
