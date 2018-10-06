using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace LabDbContext
{
    public static class TaskExtension
    {
        #region Methods

        public static void Create(this Task entry)
        {
            // Deletes given task instance

            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.Tasks.Add(entry);
                entities.SaveChanges();
            }
        }

        public static void Delete(this Task entry)
        {
            // Deletes given task instance

            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.Entry(entities.Tasks
                        .First(tsk => tsk.ID == entry.ID))
                        .State = EntityState.Deleted;

                entities.SaveChanges();
            }
        }

        public static void Update(this Task entry)
        {
            // Updates the DB values of a Task entry

            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.Tasks.AddOrUpdate(entry);
                entities.SaveChanges();
            }
        }

        #endregion Methods
    }

    public partial class Task
    {
        #region Properties

        public string BatchNumber => Batch?.Number;

        public bool? HasBatchArrived => Batch?.FirstSampleArrived;

        public bool IsAssigned => ReportID != null;

        public bool? IsComplete => Report?.IsComplete;

        public string ProjectName => Batch?.Material?.Project?.Name;

        public string RequesterName => Requester.Name;

        public string SpecificationName => SpecificationVersion?.Specification?.Standard?.Name;

        public string SpecificationVersionName => SpecificationVersion?.Name;

        public string TaskStatus
        {
            get
            {
                if (IsComplete == true)
                    return "Completo";
                else if (IsAssigned)
                    return "In Corso";
                else
                    return "Nuovo";
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Generates a list of tests based on the taskItems loading all values from the DB
        /// </summary>
        /// <param name="includeMethods">If true the related Method entities are loaded</param>
        /// <returns>An IList containing the generated test entitites</returns>
        public IList<Test> GenerateTests(bool includeMethods = false)
        {
            List<Test> output = new List<Test>();

            using (LabDbEntities entities = new LabDbEntities())
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
            using (LabDbEntities entities = new LabDbEntities())
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

        /// <summary>
        /// Reads and stores the entity property values from the DB
        /// Children collections are not loaded
        /// </summary>
        public void Load()
        {
            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                Task tempEntry = entities.Tasks.Include(tsk => tsk.Batch.Material.Aspect)
                                                .Include(tsk => tsk.Batch.Material.ExternalConstruction)
                                                .Include(tsk => tsk.Batch.Material.MaterialLine)
                                                .Include(tsk => tsk.Batch.Material.MaterialType)
                                                .Include(tsk => tsk.Batch.Material.Project.Oem)
                                                .Include(tsk => tsk.Batch.Material.Recipe.Colour)
                                                .Include(tsk => tsk.Batch.Material.Recipe.Master)
                                                .Include(tsk => tsk.Report)
                                                .Include(tsk => tsk.Requester)
                                                .Include(tsk => tsk.SpecificationVersion.Specification.Standard.Organization)
                                                .First(tsk => tsk.ID == ID);

                Batch = tempEntry.Batch;
                BatchID = tempEntry.BatchID;
                EndDate = tempEntry.EndDate;
                Notes = tempEntry.Notes;
                PipelineOrder = tempEntry.PipelineOrder;
                PriorityModifier = tempEntry.PriorityModifier;
                Progress = tempEntry.Progress;
                Report = tempEntry.Report;
                Requester = tempEntry.Requester;
                RequesterID = tempEntry.RequesterID;
                SpecificationVersion = tempEntry.SpecificationVersion;
                SpecificationVersionID = tempEntry.SpecificationVersionID;
                StartDate = tempEntry.StartDate;
            }
        }

        #endregion Methods
    }
}