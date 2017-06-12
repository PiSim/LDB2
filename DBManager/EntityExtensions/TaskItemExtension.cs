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
