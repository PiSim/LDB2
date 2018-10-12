using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace LabDbContext.EntityExtensions
{
    public static class OrganizationExtension
    {
        #region Methods
        
        public static IEnumerable<OrganizationRoleMapping> GetRoles(this Organization entry)
        {
            // Returns the RoleMappings for an organization entry

            if (entry == null)
                return null;

            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.OrganizationRoleMappings.Include(orm => orm.Role)
                                                        .Where(orm => orm.OrganizationID == entry.ID)
                                                        .ToList();
            }
        }

        #endregion Methods
    }
}