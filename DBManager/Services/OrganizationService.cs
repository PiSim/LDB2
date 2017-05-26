using System;
using System.Collections.Generic;
using System.Data.Entity;
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
                                                .ToList();
            }
        }

        public static void Load(this Organization entry)
        {
            // Loads all relevant Related Entities into a given organization entry

            if (entry == null)
                return;

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                entities.Organizations.Attach(entry);

                Organization tempEntry = entities.Organizations.Include(org => org.RoleMapping
                                                                .Select(orm => orm.Role))
                                                                .First(org => org.ID == entry.ID);

                entities.Entry(entry).CurrentValues.SetValues(tempEntry);
            }
        }

        public static void Update(this Organization entry)
        {
            // Updates the db values for a given Organization entry

            using (DBEntities entities = new DBEntities())
            {
                Organization tempEntry = entities.Organizations.First(org => org.ID == entry.ID);

                entities.Entry(entry).CurrentValues.SetValues(tempEntry);
                entities.SaveChanges();
            }
        }

        #endregion
    }
}
