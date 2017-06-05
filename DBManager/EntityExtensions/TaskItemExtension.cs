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
    }
}
