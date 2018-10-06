using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace LabDbContext
{
    public static class TaskItemExtension
    {
        #region Methods

        public static void Load(this TaskItem entry)
        {
            // Loads a TaskItem entry and all the relevant related entities

            if (entry == null)
                return;

            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                TaskItem tempEntry = entities.TaskItems.Include(tskI => tskI.SubTaskItems)
                                                        .Include(tskI => tskI.Method.Property)
                                                        .Include(tskI => tskI.Method.Standard.Organization)
                                                        .First(tskI => tskI.ID == entry.ID);

                entry.Description = tempEntry.Description;
                entry.Method = tempEntry.Method;
                entry.MethodID = tempEntry.MethodID;
                entry.Name = tempEntry.Name;
                entry.Position = tempEntry.Position;
                entry.RequirementID = tempEntry.RequirementID;
                entry.SpecificationVersionID = tempEntry.SpecificationVersionID;
                entry.SubTaskItems = tempEntry.SubTaskItems;
                entry.TaskID = tempEntry.TaskID;
                entry.TestID = tempEntry.TestID;
            }
        }

        public static void Update(this TaskItem entry)
        {
            // Updates a task Item entry

            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.TaskItems.AddOrUpdate(entry);
                entities.SaveChanges();
            }
        }

        public static void Update(this IEnumerable<TaskItem> entryList)
        {
            // Updates a list of TaskItem entries

            using (LabDbEntities entities = new LabDbEntities())
            {
                foreach (TaskItem entry in entryList)
                    entities.TaskItems.AddOrUpdate(entry);

                entities.SaveChanges();
            }
        }

        #endregion Methods
    }

    public partial class TaskItem
    {
        #region Properties

        public string MethodName => Method?.Standard?.Name;

        public string PropertyName => Method?.Property?.Name;

        #endregion Properties

        #region Methods

        /// <summary>
        /// Creates a new Test entry based on the values currently loaded in the Task instance
        /// </summary>
        /// <returns>The new Test entity</returns>
        public Test GetTest()
        {
            Test output = new Test()
            {
                MethodVariantID = Method.MethodVariants.First().ID,
                Notes = Description,
                RequirementID = RequirementID
            };

            foreach (SubTaskItem subItem in SubTaskItems)
                output.SubTests.Add(subItem.GetSubTest());

            return output;
        }

        #endregion Methods
    }
}