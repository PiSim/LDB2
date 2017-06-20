using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager.EntityExtensions
{
    public static class UserRoleExtension
    {
        public static void Create(this UserRole entry)
        {
            // Inserts a new UserRole in the DB

            using (DBEntities entities = new DBEntities())
            {
                entities.UserRoles.Add(entry);
                entities.SaveChanges();
            }
        }

    }
}
