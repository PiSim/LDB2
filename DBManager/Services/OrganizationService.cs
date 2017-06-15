using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager.Services
{
    public static class OrganizationService
    {
        #region Operations for Organization entities
        
        public static IEnumerable<Organization> GetOrganizations(string roleName = null)
        {
            // Returns all Organization entities, filtering by role if one is provided

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.Organizations.Include(org => org.RoleMapping
                                                .Select(orm => orm.Role))
                                                .Where(org => (roleName == null) ? true : org.RoleMapping
                                                .FirstOrDefault(orm => orm.Role.Name == roleName)
                                                .IsSelected)
                                                .OrderBy(org => org.Name)
                                                .ToList();
            }
        }

        #endregion
    }
}
