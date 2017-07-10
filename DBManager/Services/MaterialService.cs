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
        public static IEnumerable<Batch> GetArchive()
        {
            // Returns all the batches with a non-zero number of samples in stock

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
                                        .Where(btc => btc.ArchiveStock != 0)
                                        .OrderByDescending(btc => btc.Number)
                                        .ToList();
            }
        }

        public static IEnumerable<Batch> GetBatches()
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

        public static IEnumerable<Batch> GetBatches(int entryN)
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
                                        .OrderByDescending(btc => btc.Number)
                                        .Take(entryN)
                                        .ToList();
            }
        }

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

        public static IEnumerable<Colour> GetColours()
        {
            // Returns all Colour entities

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Colours.Where(clr => true)
                                        .OrderBy(clr => clr.Name)
                                        .ToList();
            }
        }

        public static IEnumerable<Sample> GetLatestSamples(int number = 25)
        {
            // Returns a given number of the most recently inserted samples

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Samples.Include(smp => smp.Batch)
                                        .Include(smp => smp.LogAuthor)
                                        .OrderByDescending(smp => smp.ID)
                                        .Take(number)
                                        .ToList();
            }
        }

        public static IEnumerable<Organization> GetMaintenanceOrganizations()
        {
            // Returns all organizations with the MAINT role

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;
                OrganizationRole tempRole = entities.OrganizationRoles.First(rol => rol.Name == OrganizationRoleNames.Maintenance);

                return entities.Organizations.Where(org => org.RoleMapping
                                            .FirstOrDefault(orm => orm.roleID == tempRole.ID)
                                            .IsSelected == true)
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

        public static MaterialLine GetLine(string lineCode)
        {
            // Returns a MaterialLine entry with the given code, or null if none exists

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.MaterialLines.FirstOrDefault(matl => matl.Code == lineCode);
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

        public static IEnumerable<TrialArea> GetTrialAreas()
        {
            // Returns all TrialArea entries

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.TrialAreas.Where(tra => true)
                                            .ToList();
            }
        }

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
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Batches.Include(btc => btc.Material.Aspect)
                                        .Include(btc => btc.Material.MaterialType)
                                        .Include(btc => btc.Material.MaterialLine)
                                        .Include(btc => btc.Material.Recipe.Colour)
                                        .FirstOrDefault(entry => entry.ID == ID);
            }
        }

        public static Batch GetBatch(string batchNumber)
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Batches.Include(btc => btc.Material.Aspect)
                                        .Include(btc => btc.Material.MaterialType)
                                        .Include(btc => btc.Material.MaterialLine)
                                        .Include(btc => btc.Material.Recipe.Colour)
                                        .FirstOrDefault(entry => entry.Number == batchNumber);
            }
        }

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

        public static IEnumerable<ExternalConstruction> GetExternalConstructions()
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.ExternalConstructions.Include(exc => exc.Oem)
                                                    .OrderBy(exc => exc.Oem.Name)
                                                    .ThenBy(exc => exc.Name)
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

        public static void RemoveMaterial(this ExternalConstruction entry,
                                            Material toBeRemoved)
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.AutoDetectChangesEnabled = false;

                entities.Materials.First(mat => mat.ID == toBeRemoved.ID)
                        .ExternalConstructionID = null;

                entities.SaveChanges();
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

        #region Operations for Material entities

        public static Material GetMaterial(int ID)
        {
            // Returns a Material entities with the given ID
            // if none is found null is returned

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Materials.Include(mat => mat.Aspect)
                                        .Include(mat => mat.MaterialLine)
                                        .Include(mat => mat.MaterialType)
                                        .Include(mat => mat.Recipe)
                                        .FirstOrDefault(mat => mat.ID == ID);
            }
        }

        public static Material GetMaterial(string type,
                                            string line,
                                            string aspect,
                                            string recipe)
        {
            // Returns a Material entities with the type, line, aspect and recipe
            // if none is found null is returned

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Materials.Include(mat => mat.Aspect)
                                        .Include(mat => mat.MaterialLine)
                                        .Include(mat => mat.MaterialType)
                                        .Include(mat => mat.Recipe)
                                        .FirstOrDefault(mat => mat.Aspect.Code == aspect
                                                            && mat.MaterialLine.Code == line
                                                            && mat.Recipe.Code == recipe
                                                            && mat.MaterialType.Code == type);
            }
        }

        public static Material GetMaterial(MaterialType type,
                                            MaterialLine line,
                                            Aspect aspect,
                                            Recipe recipe)
        {
            // Returns a Material entities with the given construction and recipe
            // if none is found null is returned

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Materials.Include(mat => mat.Aspect)
                                        .Include(mat => mat.MaterialLine)
                                        .Include(mat => mat.MaterialType)
                                        .Include(mat => mat.Recipe)
                                        .FirstOrDefault(mat => mat.AspectID == aspect.ID
                                                            && mat.LineID == line.ID
                                                            && mat.RecipeID == recipe.ID
                                                            && mat.TypeID == type.ID);
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
        
        
        #region Operations for Sample entities

        public static void Create(this Sample entry)
        {
            // Inserts a Sample entry in the DB

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
                                        .Include(samp => samp.Batch.Material.Aspect)
                                        .Include(samp => samp.Batch.Material.MaterialLine)
                                        .Include(samp => samp.Batch.Material.MaterialType)
                                        .Include(samp => samp.Batch.Material.Project)
                                        .Include(samp => samp.Batch.Material.Recipe.Colour)
                                        .OrderByDescending(sle => sle.Date)
                                        .Take(number)
                                        .ToList();
            }
        }

        #endregion
    }
}
