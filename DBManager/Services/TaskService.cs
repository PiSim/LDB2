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

        public static TaskItem GetTaskItem(int ID)
        {
            // Returns the Task Item with the given ID

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.TaskItems.FirstOrDefault(tski => tski.ID == ID);
            }
        }


        public static Task GetTask(int ID)
        {
            // Returns the Task Entry with the given ID, or null if none is found

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Tasks.FirstOrDefault(tsk => tsk.ID == ID);
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

        #endregion
    }
}
