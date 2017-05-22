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

                return entities.Batches.Where(btc => btc.Material.Construction.Project.ID == entry.ID)
                                        .Include(btc => btc.Material.Construction)
                                        .Include(btc => btc.Material.Construction.Aspect)
                                        .Include(btc => btc.Material.Construction.ExternalConstruction)
                                        .Include(btc => btc.Material.Construction.Type)
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

        public static IEnumerable<Project> GetProjects()
        {
            // Returns all Project entities

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Projects.Include(prj => prj.Leader)
                                        .Include(prj => prj.Oem)
                                        .ToList();
            }
        }

        public static void Create(this Project entry)
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                entities.Projects.Attach(entry);
                entities.Entry(entry).State = System.Data.Entity.EntityState.Added;
                entities.SaveChanges();
            }
        }

        public static void Delete(this Project entry)
        {
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

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Reports.Where(rep => rep.Batch.Material.Construction.Project.ID == entry.ID)
                                        .Include(rep => rep.Author)
                                        .Include(rep => rep.Batch.Material.Construction.Aspect)
                                        .Include(rep => rep.Batch.Material.Construction.Type)
                                        .Include(rep => rep.Batch.Material.Recipe.Colour)
                                        .Include(rep => rep.SpecificationVersion.Specification.Standard)
                                        .ToList();
            }
        }

        public static IEnumerable<Task> GetTasks(this Project entry)
        {
            // Returns all Task entities for a given Project

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Tasks.Where(tsk => tsk.Batch.Material.Construction.Project.ID == entry.ID)
                                    .Include(tsk => tsk.Batch.Material.Construction.Aspect)
                                    .Include(tsk => tsk.Batch.Material.Construction.Type)
                                    .Include(tsk => tsk.Batch.Material.Recipe.Colour)
                                    .Include(tsk => tsk.Requester)
                                    .ToList();
            }
        }

        public static void Load(this Project entry)
        {
            // Explicitly loads a Project and all related entities

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;
                
                entities.Projects.Attach(entry);

                Project tempEntry = entities.Projects.Include(prj => prj.Constructions
                                                    .Select(cns => cns.Aspect))
                                                    .Include(prj => prj.Constructions
                                                    .Select(cns => cns.Type))
                                                    .Include(prj => prj.ExternalReports
                                                    .Select(extr => extr.ExternalLab))
                                                    .Include(prj => prj.Leader)
                                                    .Include(prj => prj.Oem)
                                                    .First(prj => prj.ID == entry.ID);

                entities.Entry(entry).CurrentValues.SetValues(tempEntry);

            }
        }

        public static void Update(this Project entry)
        {
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
