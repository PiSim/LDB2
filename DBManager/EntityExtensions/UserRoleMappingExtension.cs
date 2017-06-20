using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager.EntityExtensions
{
    public static class UserRoleMappingExtension
    {
        public static void Create(this UserRoleMapping entry)
        {
            // Inserts a new UserRoleMapping entry

            using (DBEntities entities = new DBEntities())
            {
                entities.UserRoleMappings.Add(entry);

                entities.SaveChanges();
            }
        }

        public static void Update(this IEnumerable<UserRoleMapping> entryList)
        {
            // Updates all entries in a list of UserRoleMappings

            using (DBEntities entities = new DBEntities())
            {
                foreach (UserRoleMapping urm in entryList)
                    entities.UserRoleMappings.AddOrUpdate(urm);

                entities.SaveChanges();
            }
        }        
    }
}
