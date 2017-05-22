using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager.Services
{
    public static class MaterialService
    {
        #region Operations for Batch entities

        public static Batch GetBatch(int ID)
        {
            using (DBEntities entities = new DBEntities())
            {
                return entities.Batches.FirstOrDefault(entry => entry.ID == ID);
            }
        }

        public static Batch GetBatch(string batchNumber)
        {
            using (DBEntities entities = new DBEntities())
            {
                return entities.Batches.FirstOrDefault(entry => entry.Number == batchNumber);
            }
        }

        public static IEnumerable<Construction> GetConstructionsWithoutProject()
        {
            // returns all Construction entities unassigned to a Project

            using (DBEntities entities = new DBEntities())
            {
                return entities.Constructions.Where(cns => cns.Project == null)
                                            .Include(cns => cns.Aspect)
                                            .Include(cns => cns.ExternalConstruction)
                                            .Include(cns => cns.Type)
                                            .ToList();
            }
        }

        public static void Create(this Batch entry)
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Batches.Attach(entry);
                entities.Entry(entry).State = System.Data.Entity.EntityState.Added;
                entities.SaveChanges();
            }
        }

        public static void Delete(this Batch entry)
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Batches.Attach(entry);
                entities.Entry(entry).State = System.Data.Entity.EntityState.Deleted;
                entities.SaveChanges();
            }
        }

        public static void Load(this Batch entry)
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Batches.Attach(entry);
                entities.Configuration.LazyLoadingEnabled = false;

                Batch tempEntry = entities.Batches.Include(btc => btc.BatchFiles)
                                                    .Include(btc => btc.ExternalReports
                                                    .Select(extr => extr.ExternalLab))
                                                    .Include(btc => btc.Masters)
                                                    .Include(btc => btc.Material.Construction.Aspect)
                                                    .Include(btc => btc.Material.Construction.Project.Leader)
                                                    .Include(btc => btc.Material.Construction.Project.Oem)
                                                    .Include(btc => btc.Material.Construction.Type)
                                                    .Include(btc => btc.Material.Recipe.Colour)
                                                    .Include(btc => btc.Reports
                                                    .Select(rep => rep.Author))
                                                    .Include(btc => btc.Reports
                                                    .Select(rep => rep.SpecificationVersion.Specification.Standard))
                                                    .Include(btc => btc.Samples)
                                                    .Include(btc => btc.Tasks
                                                    .Select(tsk => tsk.SpecificationVersion.Specification.Standard))
                                                    .First(btc => btc.ID == entry.ID);

                entities.Entry(entry).CurrentValues.SetValues(tempEntry);
            }
        }

        public static void Update(this Batch entry)
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.ProxyCreationEnabled = false;

                entities.Batches.Attach(entry);
                entities.Entry(entry).State = System.Data.Entity.EntityState.Modified;
                entities.SaveChanges();
            }
        }

        #endregion

        #region Operations for Construction entities

        public static IEnumerable<Batch> GetBatches(this Construction entry)
        {
            // Gets all batches for a given Construction entity, 
            // returns empty list if instance is null

            using (DBEntities entities = new DBEntities())
            {
                if (entry == null)
                    return new List<Batch>();

                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Batches.Where(btc => btc.Material.Construction.ID == entry.ID)
                                        .Include(btc => btc.Material.Construction)
                                        .Include(btc => btc.Material.Recipe.Colour)
                                        .ToList();
            }
        }

        public static IEnumerable<Construction> GetConstructions()
        {
            // Returns all Construction entities

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Constructions.Include(con => con.Aspect)
                                            .Include(con => con.ExternalConstruction)
                                            .Include(con => con.Project)
                                            .Include(con => con.Type)
                                            .ToList();
            }
        }

        #endregion 

        #region Operations for ExternalConstruction entities

        public static void AddConstruction(this ExternalConstruction entry,
                                            Construction toBeAdded)
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.AutoDetectChangesEnabled = false;

                entities.Constructions.Attach(toBeAdded);
                toBeAdded.ExternalConstruction = entry;
                toBeAdded.ExternalConstructionID = entry.ID;
                entry.Constructions.Add(toBeAdded);

                entities.Entry(toBeAdded).State = EntityState.Modified;
                entities.SaveChanges();
            }
        }

        public static ExternalConstruction GetExternalConstruction(int ID)
        {
            using (DBEntities entities = new DBEntities())
            {
                return entities.ExternalConstructions.First(entry => entry.ID == ID);
            }
        }

        public static IEnumerable<ExternalConstruction> GetExternalConstructions()
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.ExternalConstructions.Include(exc => exc.Organization)
                                                    .ToList();
            }
        }

        public static void Create(this ExternalConstruction entry)
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.ExternalConstructions.Attach(entry);
                entities.Entry(entry).State = System.Data.Entity.EntityState.Added;
                entities.SaveChanges();
            }
        }

        public static void Delete(this ExternalConstruction entry)
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.ExternalConstructions.Attach(entry);
                entities.Entry(entry).State = System.Data.Entity.EntityState.Deleted;
                entities.SaveChanges();
            }
        }

        public static void Load(this ExternalConstruction entry)
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                int entryID = entry.ID;

                entities.ExternalConstructions.Attach(entry);

                ExternalConstruction tempEntry = entities.ExternalConstructions
                                                        .Include(extc => extc.Constructions.Select(cons => cons.Aspect))
                                                        .Include(extc => extc.Constructions.Select(cons => cons.Type))
                                                        .Include(extc => extc.Constructions)
                                                        .Include(extc => extc.DefaultSpecVersion.Specification)
                                                        .Include(extc => extc.DefaultSpecVersion.Specification.Standard)
                                                        .Include(extc => extc.Organization)
                                                        .First(extc => extc.ID == entryID);

                entities.Entry(entry).CurrentValues.SetValues(tempEntry);

            }
        }

        public static void RemoveConstruction(this ExternalConstruction entry,
                                            Construction toBeRemoved)
        {
            if (!entry.Constructions.Any(cns => cns.ID == toBeRemoved.ID))
                return;

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.AutoDetectChangesEnabled = false;

                entities.Constructions.Attach(toBeRemoved);
                toBeRemoved.ExternalConstruction = null;
                toBeRemoved.ExternalConstructionID = null;
                entry.Constructions.Remove(entry.Constructions
                                    .First(cns => cns.ID == toBeRemoved.ID));

                entities.Entry(toBeRemoved).State = EntityState.Modified;
                entities.SaveChanges();
            }
        }

        public static void Update(this ExternalConstruction entry)
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.AutoDetectChangesEnabled = false;

                ExternalConstruction tempEntry = entities.ExternalConstructions.First(extc => extc.ID == entry.ID);
                entities.Entry(tempEntry).CurrentValues.SetValues(entry);
                entities.Entry(tempEntry).State = EntityState.Modified; 
                entities.SaveChanges();
            }
        }

        #endregion

        #region Operations for Sample entities
        
        public static IEnumerable<Sample> GetRecentlyArrivedSamples(int number = 25)
        {
            using (DBEntities entities = new DBEntities())
            {
                return entities.Samples.Where(sle => sle.Code == "A")
                                        .Include(samp => samp.Batch.Material.Construction.Aspect)
                                        .Include(samp => samp.Batch.Material.Construction.Project)
                                        .Include(samp => samp.Batch.Material.Construction.Type)
                                        .Include(samp => samp.Batch.Material.Recipe.Colour)
                                        .OrderByDescending(sle => sle.Date)
                                        .Take(number)
                                        .ToList();
            }
        }

        #endregion
    }
}
