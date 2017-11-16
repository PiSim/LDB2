using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager.Services
{
    public static class ProjectService
    {
        #region Operations for Project entitites

        public static IEnumerable<Batch> GetBatches(this Project entry)
        {
            // Gets all batches for a given Project entity, 
            // returns empty list if instance is null

            using (DBEntities entities = new DBEntities())
            {
                if (entry == null)
                    return new List<Batch>();

                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Batches.Where(btc => btc.Material.ProjectID == entry.ID)
                                        .Include(btc => btc.Material.Aspect)
                                        .Include(btc => btc.Material.ExternalConstruction)
                                        .Include(btc => btc.Material.MaterialLine)
                                        .Include(btc => btc.Material.MaterialType)
                                        .Include(btc => btc.Material.Recipe.Colour)
                                        .ToList();
            }
        }

        public static Project GetProject(int ID)
        {
            // Returns a single unloaded Project instance given its ID

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Projects.First(entry => entry.ID == ID);
            }
        }

        public static Project GetProject(string name)
        {
            // Returns a Project with the given name or null if none exists

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Projects.FirstOrDefault(prj => prj.Name == name);
            }
        }

        public static IEnumerable<Project> GetProjects(bool include_collections = false)
        {
            // Returns all Project entities

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                if (include_collections)
                    return entities.Projects.Include(prj => prj.Leader)
                                            .Include(prj => prj.Oem)
                                            .Include(prj => prj.ExternalReports)
                                            .Include(prj => prj.Materials
                                            .Select(mat => mat.Batches
                                            .Select(btc => btc.Reports)))
                                            .OrderByDescending(prj => prj.Name)
                                            .ToList();


                else
                    return entities.Projects.Include(prj => prj.Leader)
                                            .Include(prj => prj.Oem)
                                            .OrderByDescending(prj => prj.Name)
                                            .ToList();
            }
        }

        #endregion  

    }
}
