using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager.EntityExtensions
{
    public static class OrganizationExtension
    {
        public static void Create(this Organization entry)
        {
            //Inserts a new Organization entry

            using (DBEntities entities = new DBEntities())
            {
                entities.Organizations.Add(entry);
                entities.SaveChanges();
            }
        }

        public static IEnumerable<OrganizationRoleMapping> GetRoles(this Organization entry)
        {
            // Returns the RoleMappings for an organization entry

            if (entry == null)
                return null;

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.OrganizationRoleMappings.Include(orm => orm.Role)
                                                        .Where(orm => orm.OrganizationID == entry.ID)
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

                Organization tempEntry = entities.Organizations.Include(org => org.RoleMapping
                                                                .Select(orm => orm.Role))
                                                                .First(org => org.ID == entry.ID);

                entry.Name = tempEntry.Name;
                entry.RoleMapping = tempEntry.RoleMapping;
            }
        }

        public static void Update(this IEnumerable<OrganizationRoleMapping> entries)
        {
            // updates a list of OrganizationRoleMapping entries

            using (DBEntities entities = new DBEntities())
            {
                foreach (OrganizationRoleMapping orm in entries)
                    entities.OrganizationRoleMappings.AddOrUpdate(orm);

                entities.SaveChanges();
            }
        }

        public static void Update(this Organization entry)
        {
            // Updates the db values for a given Organization entry

            using (DBEntities entities = new DBEntities())
            {
                entities.Organizations.AddOrUpdate(entry);

                entities.SaveChanges();
            }
        }
    }
}
