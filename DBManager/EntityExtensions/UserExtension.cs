using DBManager;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager.EntityExtensions
{
    public static class UserExtension
    {
        public static void Update(this User entry)
        {
            // Updates a User Entry

            using (DBEntities entities = new DBEntities())
            {
                entities.Users.AddOrUpdate(entry);

                entities.SaveChanges();
            }
        }

    }
}
