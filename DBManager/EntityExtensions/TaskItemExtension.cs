using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager.EntityExtensions
{
    public static class TaskItemExtension
    {

        public static void Load(this TaskItem entry)
        {
            // Loads a TaskItem entry and all the relevant related entities 

            if (entry == null)
                return;

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                TaskItem tempEntry = entities.TaskItems.Include(tskI => tskI.SubTaskItems)
                                                        .Include(tskI => tskI.Method.Property)
                                                        .Include(tskI => tskI.Method.Standard.CurrentIssue)
                                                        .Include(tskI => tskI.Method.Standard.Organization)
                                                        .First(tskI => tskI.ID == entry.ID);

                entry.Description = tempEntry.Description;
                entry.IsAssignedToReport = tempEntry.IsAssignedToReport;
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

            using (DBEntities entities = new DBEntities())
            {
                entities.TaskItems.AddOrUpdate(entry);
                entities.SaveChanges();
            }
        }

        public static void Update(this IEnumerable<TaskItem> entryList)
        {
            // Updates a list of TaskItem entries

            using (DBEntities entities = new DBEntities())
            {
                foreach (TaskItem entry in entryList)
                    entities.TaskItems.AddOrUpdate(entry);

                entities.SaveChanges();
            }
        }
    }
}
