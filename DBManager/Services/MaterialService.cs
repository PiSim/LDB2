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
                return entities.Batches.First(entry => entry.ID == ID);
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

                entities.Entry(entry).Collection(ent => ent.BatchFiles).Load();
                entities.Entry(entry).Collection(ent => ent.ExternalReportMapping).Load();
                entities.Entry(entry).Collection(ent => ent.Reports).Load();
                entities.Entry(entry).Collection(ent => ent.Tasks).Load();

                entities.Entry(entry).Reference(ent => ent.Material.Construction.Aspect).Load();
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
    }
}
