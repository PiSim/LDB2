using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager.Services
{
    public static class MaterialService
    {
        
        public static Colour GetColour(string name)
        {
            // Returns a Colour instance with the given name, or null if none exists

            if (string.IsNullOrEmpty(name))
                return null;

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Colours.FirstOrDefault(col => col.Name == name);
            }
        }

        public static IEnumerable<Batch> GetLongTermStorage()
        {
            // Returns all the batches with a non-zero number of samples in long term storage

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Batches.Include(btc => btc.BasicReport)
                                        .Include(btc => btc.FirstSample)
                                        .Include(btc => btc.Material.Aspect)
                                        .Include(btc => btc.Material.MaterialLine)
                                        .Include(btc => btc.Material.MaterialType)
                                        .Include(btc => btc.Material.Recipe.Colour)
                                        .Include(btc => btc.Material.ExternalConstruction.Oem)
                                        .Include(btc => btc.TrialArea)
                                        .Where(btc => btc.LongTermStock != 0)
                                        .OrderByDescending(btc => btc.Number)
                                        .ToList();
            }
        }

        public static IEnumerable<Material> GetMaterials()
        {
            // Returns all material entities 

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;                                

                return entities.Materials.Include(mat => mat.Aspect)
                                        .Include(mat => mat.MaterialLine)
                                        .Include(mat => mat.MaterialType)
                                        .Include(mat => mat.Recipe.Colour)
                                        .Include(mat => mat.Project)
                                        .Where(mat => true)
                                        .OrderBy(mat => mat.MaterialType.Code)
                                        .ThenBy(mat => mat.MaterialLine.Code)
                                        .ToList();
            }
        }


        public static IEnumerable<Material> GetMaterialsWithoutProject()
        {
            // Returns all Material entities without a project

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Materials.Include(mat => mat.Aspect)
                                        .Include(mat => mat.MaterialLine)
                                        .Include(mat => mat.MaterialType)
                                        .Include(mat => mat.Recipe.Colour)
                                        .Where(mat => mat.Project == null)
                                        .ToList();
            }
        }

        #region Operations for Aspect entities


        #endregion
        

        #region Operations for ExternalConstruction entities

        public static void AddMaterial(this ExternalConstruction entry,
                                            Material toBeAdded)
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Materials.First(mat => mat.ID == toBeAdded.ID)
                                    .ExternalConstructionID = entry.ID;

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
                entities.Entry(entities
                        .ExternalConstructions
                        .First(exc => exc.ID == entry.ID))
                        .State = System.Data.Entity.EntityState.Deleted;
                entities.SaveChanges();

                entry.ID = 0;
            }
        }

        public static void Load(this ExternalConstruction entry)
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                int entryID = entry.ID;


                ExternalConstruction tempEntry = entities.ExternalConstructions
                                                        .Include(extc => extc.DefaultSpecVersion.Specification)
                                                        .Include(extc => extc.DefaultSpecVersion.Specification.Standard)
                                                        .Include(extc => extc.Oem)
                                                        .First(extc => extc.ID == entryID);
                
                entry.DefaultSpecVersion = tempEntry.DefaultSpecVersion;
                entry.DefaultSpecVersionID = tempEntry.DefaultSpecVersionID;
                entry.Name = tempEntry.Name;
                entry.OemID = tempEntry.OemID;
                entry.Oem = tempEntry.Oem;
            }
        }

        public static void Update(this ExternalConstruction entry)
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.ExternalConstructions.AddOrUpdate(entry);
                entities.SaveChanges();
            }
        }

        #endregion
        
    }
}
