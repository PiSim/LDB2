using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager
{
    public partial class Task
    {
        /// <summary>
        /// Generates a list of tests based on the taskItems loading all values from the DB
        /// </summary>
        /// <param name="includeMethods">If true the related Method entities are loaded</param>
        /// <returns>An IList containing the generated test entitites</returns>
        public IList<Test> GenerateTests(bool includeMethods = false)
        {
            List<Test> output = new List<Test>();

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                IEnumerable<TaskItem> _itemList;

                if (includeMethods)
                    _itemList = entities.TaskItems
                                        .Include(tski => tski.Method.Property)
                                        .Include(tski => tski.Method.Standard)
                                        .Include(tski => tski.SubTaskItems)
                                        .Where(tski => tski.TaskID == ID)
                                        .ToList();

                else
                    _itemList = entities.TaskItems
                                        .Include(tski => tski.SubTaskItems)
                                        .Where(tski => tski.TaskID == ID)
                                        .ToList();

                foreach (TaskItem currentItem in _itemList)
                    output.Add(currentItem.GetTest());

                return output;
            }
        }

        /// <summary>
        /// Returns an IEnumerable containing all the TaskItem entries related to this Task,
        /// including subtasks and the relevant Method entities
        /// </summary>
        public IEnumerable<TaskItem> GetTaskItems()
        {
            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.TaskItems
                                .Include(tski => tski.Method.Property)
                                .Include(tski => tski.Method.Standard)
                                .Include(tski => tski.SubTaskItems)
                                .Where(tski => tski.TaskID == ID)
                                .ToList();
            }
        }
    }

    public static class TaskExtension
    {

        public static void Create(this Task entry)
        {
            // Deletes given task instance

            using (DBEntities entities = new DBEntities())
            {
                entities.Tasks.Add(entry);
                entities.SaveChanges();
            }
        }

        public static void Delete(this Task entry)
        {
            // Deletes given task instance

            using (DBEntities entities = new DBEntities())
            {
                entities.Entry(entities.Tasks
                        .First(tsk => tsk.ID == entry.ID))
                        .State = EntityState.Deleted;

                entities.SaveChanges();
            }
        }

        public static void Load(this Task entry)
        {
            // Loads all relevant Related Entities for given task instance

            if (entry == null)
                return;

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;
                
                Task tempEntry = entities.Tasks.Include(tsk => tsk.Batch.Material.Aspect)
                                                .Include(tsk => tsk.Batch.Material.MaterialLine)
                                                .Include(tsk => tsk.Batch.Material.MaterialType)
                                                .Include(tsk => tsk.Batch.Material.Project.Oem)
                                                .Include(tsk => tsk.Batch.Material.Recipe.Colour)
                                                .Include(tsk => tsk.Batch.Material.Recipe.Master)
                                                .Include(tsk => tsk.Requester)
                                                .Include(tsk => tsk.SpecificationVersion.Specification.Standard.Organization)
                                                .Include(tsk => tsk.TaskItems
                                                .Select(tski => tski.SubTaskItems))
                                                .Include(tsk => tsk.TaskItems
                                                .Select(tski => tski.Test.Report))
                                                .Include(tsk => tsk.TaskItems
                                                .Select(tski => tski.Requirement.Method.Standard))
                                                .Include(tsk => tsk.TaskItems
                                                .Select(tski => tski.Requirement.Method.Property))
                                                .First(tsk => tsk.ID == entry.ID);

                entry.AllItemsAssigned = tempEntry.AllItemsAssigned;
                entry.Batch = tempEntry.Batch;
                entry.BatchID = tempEntry.BatchID;
                entry.EndDate = tempEntry.EndDate;
                entry.IsComplete = tempEntry.IsComplete;
                entry.Notes = tempEntry.Notes;
                entry.PipelineOrder = tempEntry.PipelineOrder;
                entry.PriorityModifier = tempEntry.PriorityModifier;
                entry.Progress = tempEntry.Progress;
                entry.Reports = tempEntry.Reports;
                entry.Requester = tempEntry.Requester;
                entry.RequesterID = tempEntry.RequesterID;
                entry.SpecificationVersion = tempEntry.SpecificationVersion;
                entry.SpecificationVersionID = tempEntry.SpecificationVersionID;
                entry.StartDate = tempEntry.StartDate;
                entry.TaskItems = tempEntry.TaskItems;
            }
        }

        public static void SetAssigned(this Task entry,
                                        bool isAssigned)
        {
            // Sets the Assignment flag of a Task entry

            using (DBEntities entities = new DBEntities())
            {
                entry.AllItemsAssigned = isAssigned;
                entities.Tasks.AddOrUpdate(entry);
                entities.SaveChanges();
            }
        }
        
        public static void SetComplete(this Task entry,
                                        bool isComplete)
        {
            // Sets the Assignment flag of a Task entry

            using (DBEntities entities = new DBEntities())
            {
                entry.IsComplete = isComplete;
                entities.Tasks.AddOrUpdate(entry);
                entities.SaveChanges();
            }
        }

        public static void Update(this Task entry)
        {
            // Updates the DB values of a Task entry

            using (DBEntities entities = new DBEntities())
            {
                entities.Tasks.AddOrUpdate(entry);
                entities.SaveChanges();
            }
        }
    }
}
