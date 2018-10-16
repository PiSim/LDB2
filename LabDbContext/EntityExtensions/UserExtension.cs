using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace LabDbContext.EntityExtensions
{
    public static class UserExtension
    {
        #region Methods
        [Obsolete]
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

        #endregion Methods
    }
}