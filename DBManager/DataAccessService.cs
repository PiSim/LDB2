using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager
{
    public class DataAccessService : IDataService
    {

        public DataAccessService()
        {

        }
        
        public IEnumerable<Batch> GetBatches()
        {
            // Returns all Batches

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Batches.Include(btc => btc.BasicReport)
                                        .Include(btc => btc.FirstSample)
                                        .Include(btc => btc.Material.Aspect)
                                        .Include(btc => btc.Material.ExternalConstruction.Oem)
                                        .Include(btc => btc.Material.MaterialLine)
                                        .Include(btc => btc.Material.MaterialType)
                                        .Include(btc => btc.Material.Recipe.Colour)
                                        .Include(btc => btc.TrialArea)
                                        .Where(btc => true)
                                        .OrderByDescending(btc => btc.Number)
                                        .ToList();
            }
        }

        public IEnumerable<Batch> GetBatches(int numberOfEntries)
        {
            // Returns the first numberOfEntries Batches by number descending

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Batches.Include(btc => btc.BasicReport)
                                        .Include(btc => btc.FirstSample)
                                        .Include(btc => btc.Material.Aspect)
                                        .Include(btc => btc.Material.ExternalConstruction.Oem)
                                        .Include(btc => btc.Material.MaterialLine)
                                        .Include(btc => btc.Material.MaterialType)
                                        .Include(btc => btc.Material.Recipe.Colour)
                                        .Include(btc => btc.TrialArea)
                                        .OrderByDescending(btc => btc.Number)
                                        .Take(numberOfEntries)
                                        .ToList();
            }
        }
        
        public IList<T> GetQueryResults<T>(IQuery<T> query)
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return query.RunQuery(entities)
                            .ToList();
            }
        }
    }
}
