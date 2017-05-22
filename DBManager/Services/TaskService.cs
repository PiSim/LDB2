using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager.Services
{
    public static class TaskService
    {
        #region Operations for Task entities

        public static void Create(this Task entry)
        {
            // Deletes given task instance

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                entities.Tasks.Add(entry);
                entities.SaveChanges();
            }
        }

        public static void Delete(this Task entry)
        {
            // Deletes given task instance

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                entities.Tasks.Attach(entry);
                entities.Entry(entry).State = System.Data.Entity.EntityState.Deleted;
                entities.SaveChanges();
            }
        }

        public static IEnumerable<Task> GetTasks(bool includeComplete = true,
                                                bool includeAssigned = true)
        {
            // returns all Task entities. Bool parameter allows to filter results marked as complete

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Tasks.Include(tsk => tsk.Batch.Material.Construction.Aspect)
                                    .Include(tsk => tsk.Batch.Material.Construction.Project)
                                    .Include(tsk => tsk.Batch.Material.Construction.Type)
                                    .Include(tsk => tsk.Batch.Material.Recipe.Colour)
                                    .Include(tsk => tsk.Requester)
                                    .Include(tsk => tsk.SpecificationVersion.Specification.Standard)
                                    .Where(tsk => tsk.IsComplete == false || tsk.IsComplete == includeComplete)
                                    .Where(tsk => tsk.AllItemsAssigned == false || tsk.AllItemsAssigned == includeAssigned)
                                    .ToList();
            }

        }

        public static void Load(this Task entry)
        {
            // Loads all relevant Related Entities for given task instance

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                entities.Tasks.Attach(entry);

                Task tempEntry = entities.Tasks.Include(tsk => tsk.Batch.Material.Construction.Aspect)
                                                .Include(tsk => tsk.Batch.Material.Construction.Project.Oem)
                                                .Include(tsk => tsk.Batch.Material.Construction.Type)
                                                .Include(tsk => tsk.Batch.Material.Recipe.Colour)
                                                .Include(tsk => tsk.Batch.Material.Recipe.Master)
                                                .Include(tsk => tsk.Requester)
                                                .Include(tsk => tsk.SpecificationVersion.Specification.Standard.CurrentIssue)
                                                .Include(tsk => tsk.SpecificationVersion.Specification.Standard.Organization)
                                                .Include(tsk => tsk.TaskItems
                                                .Select(tski => tski.Test.Report))
                                                .Include(tsk => tsk.TaskItems
                                                .Select(tski => tski.Requirement.Method.Standard))
                                                .Include(tsk => tsk.TaskItems
                                                .Select(tski => tski.Requirement.Method.Property))
                                                .First(tsk => tsk.ID == entry.ID);

                entities.Entry(entry).CurrentValues.SetValues(tempEntry);
            }
        }

        #endregion
    }
}
