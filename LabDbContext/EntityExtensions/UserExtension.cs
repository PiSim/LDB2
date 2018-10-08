using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace LabDbContext.EntityExtensions
{
    public static class UserExtension
    {
        #region Methods

        public static void Delete(this User entry)
        {
            // Deletes a User entry

            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.Entry(entities.Users
                        .First(usr => usr.ID == entry.ID))
                        .State = EntityState.Deleted;

                entities.SaveChanges();

                entry.ID = 0;
            }
        }

        public static IEnumerable<UserRoleMapping> GetRoles(this User entry)
        {
            // Returns all UserRoleMapping entities for a given User entry

            if (entry == null)
                return null;

            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.UserRoleMappings.Include(usrm => usrm.UserRole)
                                                .Where(usrm => usrm.UserID == entry.ID)
                                                .ToList();
            }
        }

        public static void Update(this User entry)
        {
            // Updates a User Entry

            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.Users.AddOrUpdate(entry);

                entities.SaveChanges();
            }
        }

        #endregion Methods
    }
}