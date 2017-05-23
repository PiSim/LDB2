using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager.Services
{
    public class PeopleService
    {
        #region Operations for People entities

        public static IEnumerable<Person> GetProjectLeaders()
        {
            // Returns all People entities with ProjectLeader Role selected

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.PersonRoleMappings.Where(prm => prm.IsSelected && prm.Role == entities.PersonRoles
                                                    .First(role => role.Name == PersonRoleNames.ProjectLeader))
                                                    .Select(prm => prm.Person)
                                                    .ToList();            
            }
        }

        #endregion

    }
}
