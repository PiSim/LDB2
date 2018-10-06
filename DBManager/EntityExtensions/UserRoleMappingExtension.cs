using System.Collections.Generic;
using System.Data.Entity.Migrations;

namespace LabDbContext.EntityExtensions
{
    public static class UserRoleMappingExtension
    {
        #region Methods

        public static void Create(this UserRoleMapping entry)
        {
            // Inserts a new UserRoleMapping entry

            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.UserRoleMappings.Add(entry);

                entities.SaveChanges();
            }
        }

        public static void Update(this IEnumerable<UserRoleMapping> entryList)
        {
            // Updates all entries in a list of UserRoleMappings

            using (LabDbEntities entities = new LabDbEntities())
            {
                foreach (UserRoleMapping urm in entryList)
                    entities.UserRoleMappings.AddOrUpdate(urm);

                entities.SaveChanges();
            }
        }

        #endregion Methods
    }
}