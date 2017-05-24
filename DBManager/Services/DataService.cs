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

        public static IEnumerable<Organization> GetOEMs()
        {
            using (var entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.OrganizationRoleMappings
                            .Where(orm => orm.Role.Name == OrganizationRoleNames.OEM && orm.IsSelected)
                            .Select(orm => orm.Organization)
                            .OrderBy(org => org.Name)
                            .ToList();
            }
        }
    }
}
