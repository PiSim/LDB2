using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace LabDbContext.EntityExtensions
{
    public static class OrganizationExtension
    {
        #region Methods

        public static void Create(this Organization entry)
        {
            //Inserts a new Organization entry

            using (LabDbEntities entities = new LabDbEntities())
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

            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.OrganizationRoleMappings.Include(orm => orm.Role)
                                                        .Where(orm => orm.OrganizationID == entry.ID)
                                                        .ToList();
            }
        }

        public static void Update(this IEnumerable<OrganizationRoleMapping> entries)
        {
            // updates a list of OrganizationRoleMapping entries

            using (LabDbEntities entities = new LabDbEntities())
            {
                foreach (OrganizationRoleMapping orm in entries)
                    entities.OrganizationRoleMappings.AddOrUpdate(orm);

                entities.SaveChanges();
            }
        }

        public static void Update(this Organization entry)
        {
            // Updates the db values for a given Organization entry

            using (LabDbEntities entities = new LabDbEntities())
            {
                entities.Organizations.AddOrUpdate(entry);

                entities.SaveChanges();
            }
        }

        #endregion Methods
    }
}