using System;
using System.Collections.Generic;
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

                if (roleName != null)
                    return entities.OrganizationRoleMappings.Where(orm => orm.IsSelected && orm.Role == entities.OrganizationRoles
                                                            .First(orgr => orgr.Name == roleName))
                                                            .Select(orm => orm.Organization)
                                                            .ToList();

                else
                    return entities.Organizations.ToList();
            }
        }

        #endregion
    }
}
