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

        public static IEnumerable<Project> GetProjects()
        {
            // Returns all Project entities

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Projects.Include(prj => prj.Leader)
                                        .Include(prj => prj.Oem)
                                        .OrderByDescending(prj => prj.Name)
                                        .ToList();
            }
        }

        public static void Create(this Project entry)
        {
            if (entry == null)
                throw new NullReferenceException();

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                entities.Projects.Add(entry);
                entities.SaveChanges();
            }
        }

        public static void Delete(this Project entry)
        {
            if (entry == null)
                throw new NullReferenceException();

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                entities.Projects.Attach(entry);
                entities.Entry(entry).State = System.Data.Entity.EntityState.Deleted;
                entities.SaveChanges();
            }
        }

        public static IEnumerable<Report> GetReports(this Project entry)
        {
            // Returns all Report entities for a given Project

            if (entry == null)
                throw new NullReferenceException();

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Reports.Where(rep => rep.Batch.Material.ProjectID == entry.ID)
                                        .Include(rep => rep.Author)
                                        .Include(rep => rep.Batch.Material.Aspect)
                                        .Include(rep => rep.Batch.Material.MaterialLine)
                                        .Include(rep => rep.Batch.Material.MaterialType)
                                        .Include(rep => rep.Batch.Material.Recipe.Colour)
                                        .Include(rep => rep.SpecificationVersion.Specification.Standard)
                                        .ToList();
            }
        }

        public static IEnumerable<Task> GetTasks(this Project entry)
        {
            // Returns all Task entities for a given Project

            if (entry == null)
                throw new NullReferenceException();

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Tasks.Where(tsk => tsk.Batch.Material.ProjectID == entry.ID)
                                    .Include(tsk => tsk.Batch.Material.Aspect)
                                    .Include(tsk => tsk.Batch.Material.MaterialLine)
                                    .Include(tsk => tsk.Batch.Material.MaterialType)
                                    .Include(tsk => tsk.Batch.Material.Recipe.Colour)
                                    .Include(tsk => tsk.Requester)
                                    .ToList();
            }
        }

        public static void Load(this Project entry)
        {
            // Explicitly loads a Project and all related entities

            if (entry == null)
                return;

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;
                
                Project tempEntry = entities.Projects.Include(prj => prj.ExternalReports
                                                    .Select(extr => extr.ExternalLab))
                                                    .Include(prj => prj.Leader)
                                                    .Include(prj => prj.Oem)
                                                    .First(prj => prj.ID == entry.ID);
                
                entry.Description = tempEntry.Description;
                entry.ExternalReports = tempEntry.ExternalReports;
                entry.Leader = tempEntry.Leader;
                entry.Name = tempEntry.Name;
                entry.Oem = tempEntry.Oem;
                entry.OemID = tempEntry.OemID;
                entry.ProjectLeaderID = tempEntry.ProjectLeaderID;
                entry.TotalExternalCost = tempEntry.TotalExternalCost;
                entry.TotalInternalCost = tempEntry.TotalInternalCost;
            }
        }

        public static void Update(this Project entry)
        {

            if (entry == null)
                throw new NullReferenceException();

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;
                entities.Configuration.AutoDetectChangesEnabled = false;

                Project tempEntry = entities.Projects.First(prj => prj.ID == entry.ID);
                entities.Entry(tempEntry).CurrentValues.SetValues(entry);
                entities.Entry(tempEntry).State = EntityState.Modified;
                entities.SaveChanges();
            }
        }

        #endregion  

    }
}
