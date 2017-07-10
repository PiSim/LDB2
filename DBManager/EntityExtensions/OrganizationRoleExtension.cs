using DBManager;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager.EntityExtensions
{
    public static class OrganizationRoleExtension
    {
        public static void Create(this OrganizationRole entry)
        {
            // Inserts a new OrganizationRole entry in the DB

            using (DBEntities entities = new DBEntities())
            {
                entities.OrganizationRoles.Add(entry);

                entities.SaveChanges();
            }
        }

    }
}
