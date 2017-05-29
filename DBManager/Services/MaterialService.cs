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
        #region Operations for Aspect entities

        public static Aspect GetAspect(string code)
        {
            // Returns an Aspect entity with the given code
            // if none is found null is returned

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Aspects.FirstOrDefault(asp => asp.Code == code);
            }
        }

        public static IEnumerable<Aspect> GetAspects()
        {
            // Returns all Aspect entities

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Aspects.Where(asp => true)
                                        .ToList();
            }
        }

        #endregion

        #region Operations for Batch entities

        public static Batch GetBatch(int ID)
        {
            using (DBEntities entities = new DBEntities())
            {
                return entities.Batches.Include(btc => btc.Material.Construction.Aspect)
                                        .Include(btc => btc.Material.Construction.Type)
                                        .Include(btc => btc.Material.Recipe.Colour)
                                        .FirstOrDefault(entry => entry.ID == ID);
            }
        }

        public static Batch GetBatch(string batchNumber)
        {
            using (DBEntities entities = new DBEntities())
            {
                return entities.Batches.Include(btc => btc.Material.Construction.Aspect)
                                        .Include(btc => btc.Material.Construction.Type)
                                        .Include(btc => btc.Material.Recipe.Colour)
                                        .FirstOrDefault(entry => entry.Number == batchNumber);
            }
        }

        public static void Create(this Batch entry)
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Batches.Add(entry);
                entities.SaveChanges();
            }
        }

        public static void Delete(this Batch entry)
        {
            using (DBEntities entities = new DBEntities())
            {
                Batch tempEntry = entities.Batches.First(btc => btc.ID == entry.ID);

                entities.Entry(tempEntry).State = EntityState.Deleted;

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
            // Updates the DB values of a Batch entity

            using (DBEntities entities = new DBEntities())
            {
                Batch tempEntry = entities.Batches.First(btc => btc.ID == entry.ID);

                if (tempEntry.Material != null && tempEntry.MaterialID == 0)
                    tempEntry.MaterialID = tempEntry.Material.ID;

                entities.Entry(tempEntry).CurrentValues.SetValues(entry);

                entities.SaveChanges();
            }
        }

        #endregion

        #region Operations for Colour entities

        public static IEnumerable<Colour> GetColours()
        {
            // Returns all Colour entities

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Colours.Where(clr => true)
                                        .ToList();
            }
        }

        #endregion

        #region Operations for Construction entities

        public static void Create(this Construction entry)
        {
            if (entry == null)
                throw new NullReferenceException();

            using (DBEntities entities = new DBEntities())
            {
                if (entry.AspectID == 0)
                    entry.AspectID = entry.Aspect.ID;

                if (entry.TypeID == 0)
                    entry.TypeID = entry.Type.ID;

                Construction newEntry = new Construction();

                entities.Constructions.Add(newEntry);
                entities.Entry(newEntry).CurrentValues.SetValues(entry);
                entities.SaveChanges();

                entry.ID = newEntry.ID;
            }
        }

        public static void Delete(this Construction entry)
        {
            // Deletes an aspect entity

            if (entry == null)
                throw new NullReferenceException();

            using (DBEntities entities = new DBEntities())
            {
                entities.Constructions.Attach(entry);
                entities.Entry(entry).State = EntityState.Deleted;
                entities.SaveChanges();
            }
        }

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

        public static Construction GetConstruction(int ID)
        {
            // Returns a construction with the given ID
            // if none is found in the DB, null is returned
            
            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Constructions.FirstOrDefault(con => con.ID == ID);
            }
        }

        public static Construction GetConstruction(string typeCode,
                                                    string line,
                                                    string aspectCode)
        {
            // Returns a construction with the given typeCode, line and aspectCode
            // if none is found in the DB, null is returned

            if (typeCode == null || line == null || aspectCode == null)
                throw new NullReferenceException();

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Constructions.FirstOrDefault(con => con.Aspect.Code == aspectCode
                                                                && con.Line == line
                                                                && con.Type.Code == typeCode);
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

        public static void SetType(this Construction entry, 
                                    MaterialType typeEntity)
        {
            entry.Type = typeEntity;
            entry.TypeID = (typeEntity == null) ? 0 : typeEntity.ID;
        }

        public static void Update(this Construction entry)
        {
            using (DBEntities entities = new DBEntities())
            {

                Construction tempEntry = entities.Constructions.First(con => con.ID == entry.ID);

                entities.Entry(tempEntry).CurrentValues.SetValues(entry);
                
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


                ExternalConstruction tempEntry = entities.ExternalConstructions
                                                        .Include(extc => extc.Constructions.Select(cons => cons.Aspect))
                                                        .Include(extc => extc.Constructions.Select(cons => cons.Type))
                                                        .Include(extc => extc.Constructions)
                                                        .Include(extc => extc.DefaultSpecVersion.Specification)
                                                        .Include(extc => extc.DefaultSpecVersion.Specification.Standard)
                                                        .Include(extc => extc.Organization)
                                                        .First(extc => extc.ID == entryID);

                entry.Constructions = tempEntry.Constructions;
                entry.DefaultSpecVersion = tempEntry.DefaultSpecVersion;
                entry.DefaultSpecVersionID = tempEntry.DefaultSpecVersionID;
                entry.Name = tempEntry.Name;
                entry.OemID = tempEntry.OemID;
                entry.Organization = tempEntry.Organization;
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

        #region Operations for Material entities

        public static Material GetMaterial(int ID)
        {
            // Returns a Material entities with the given ID
            // if none is found null is returned

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Materials.Include(mat => mat.Construction.Aspect)
                                        .Include(mat => mat.Construction.Type)
                                        .Include(mat => mat.Recipe)
                                        .FirstOrDefault(mat => mat.ID == ID);
            }
        }

        public static Material GetMaterial(Construction construction,
                                            Recipe recipe)
        {
            // Returns a Material entities with the given construction and recipe
            // if none is found null is returned

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Materials.Include(mat => mat.Construction.Aspect)
                                        .Include(mat => mat.Construction.Type)
                                        .Include(mat => mat.Recipe)
                                        .FirstOrDefault(mat => mat.ConstructionID == construction.ID
                                                            && mat.RecipeID == recipe.ID);
            }
        }

        #endregion

        #region Operations for MaterialType entities

        public static MaterialType GetMaterialType(string code)
        {
            // Returns a MaterialType entity with the given code
            // if none is found null is returned

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.MaterialTypes.FirstOrDefault(mty => mty.Code == code);
            }
        }

        #endregion

        #region Operations for Recipe entities

        public static void Create(this Recipe entry)
        {
            // Inserts a new Recipe entity in the DB

            using (DBEntities entities = new DBEntities())
            {
                entities.Recipes.Add(entry);
                entities.SaveChanges();
            }
        }

        public static Recipe GetRecipe(string code)
        {
            // Returns the recipe with the given code
            // if none is found, null is returned

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Recipes.FirstOrDefault(rec => rec.Code == code);
            }
        }

        #endregion

        #region Operations for Sample entities

        public static void Create(this Sample entry)
        {
            // Inserts a Sample entry in the DB

            if (entry == null)
                throw new NullReferenceException();

            using (DBEntities entities = new DBEntities())
            {
                entities.Samples.Add(entry);

                entities.SaveChanges();
            }
        }

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
