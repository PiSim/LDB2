using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBManager.Services
{
    public static class PeopleService
    {
        #region Operations for People entities

        public static IEnumerable<Person> GetPeople(string roleName = null)
        {
            // Returns all People entities, a rolename can be provided to filter by

            using (DBEntities entities = new DBEntities())
            {

                entities.Configuration.LazyLoadingEnabled = false;

                return entities.People.Where(per => roleName == null || per.RoleMappings
                                        .First(prm => prm.Role.Name == roleName)
                                        .IsSelected)
                                        .ToList();
            }
        }

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

        public static void Load(this Person entry)
        {
            // Loads the relevant Related Entities into a given Person Instance

            if (entry == null)
                return;

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                entities.People.Attach(entry);

                Person tempEntry = entities.People.Include(per => per.RoleMappings
                                                    .Select(prm => prm.Role))
                                                    .First(per => per.ID == entry.ID);

                entities.Entry(entry).CurrentValues.SetValues(entry);
            }
        }

        public static void Update(this Person entry)
        {
            // Updates the DB values of a given entry

            if (entry == null)
                return;

            using (DBEntities entities = new DBEntities())
            {
                Person tempEntry = entities.People.First(per => per.ID == entry.ID);

                entities.Entry(tempEntry).CurrentValues.SetValues(entry);

                entities.SaveChanges();
            }
        }

        #endregion

        #region Operations for PersonRole entities

        public static IEnumerable<PersonRole> GetPersonRoles()
        {
            // Returns all PersonRole entities

            using (DBEntities entities = new DBEntities())
            {
                entities.Configuration.LazyLoadingEnabled = false;

                return entities.PersonRoles.Where(prol => true)
                                            .ToList();
            }
        }

        #endregion
    }
}
