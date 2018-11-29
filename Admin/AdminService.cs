using Admin.Queries;
using Controls.Views;
using DataAccessCore;
using DataAccessCore.Commands;
using Infrastructure;
using Infrastructure.Events;
using LInst;
using Microsoft.EntityFrameworkCore.Design;
using Prism.Events;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace Admin
{
    public class AdminService : IAdminService
    {
        #region Fields
        
        private IEventAggregator _eventAggregator;
        private IDataService<LInstContext> _lInstData;
        IDesignTimeDbContextFactory<LInstContext> _dbContextFactory;

        #endregion Fields

        #region Constructors

        public AdminService(IEventAggregator aggregator,
                            IDesignTimeDbContextFactory<LInstContext> dbContextFactory,
                            IDataService<LInstContext> lInstData)
        {
            _eventAggregator = aggregator;
            _dbContextFactory = dbContextFactory;
            _lInstData = lInstData;
        }

        #endregion Constructors

        #region Methods

        public void AddOrganizationRole(string name)
        {
            using (LInstContext entities = _dbContextFactory.CreateDbContext(new string[]{ }))
            {
                OrganizationRole newRole = new OrganizationRole
                {
                    Name = name,
                    Description = ""
                };

                entities.OrganizationRoles.Add(newRole);

                foreach (Organization org in entities.Organizations)
                {
                    OrganizationRoleMapping newMapping = new OrganizationRoleMapping
                    {
                        OrganizationRole = newRole,
                        Organization = org,
                        IsSelected = false
                    };
                    entities.OrganizationRoleMappings.Add(newMapping);
                }

                entities.SaveChanges();
            }
        }

        public InstrumentType CreateNewInstrumentType()
        {
            StringInputDialog creationDialog = new StringInputDialog
            {
                Title = "Crea nuovo Tipo strumenti"
            };

            if (creationDialog.ShowDialog() == true)
            {
                InstrumentType output = new InstrumentType()
                {
                    Name = creationDialog.InputString
                };
                
                _lInstData.Execute(new InsertEntityCommand<LInstContext>(output));
                return output;
            }

            return null;
        }

        public Organization CreateNewOrganization()
        {
            StringInputDialog creationDialog = new StringInputDialog
            {
                Title = "Crea nuovo Ente"
            };

            if (creationDialog.ShowDialog() == true)
            {
                Organization output = new Organization
                {
                    Name = creationDialog.InputString
                };
                foreach (OrganizationRole orr in _lInstData.RunQuery(new OrganizationRolesQuery()))
                {
                    OrganizationRoleMapping tempORM = new OrganizationRoleMapping
                    {
                        IsSelected = false,
                        OrganizationRoleID = orr.ID
                    };

                    output.RoleMappings.Add(tempORM);
                }

                _lInstData.Execute(new InsertEntityCommand<LInstContext>(output));

                EntityChangedToken token = new EntityChangedToken(output,
                                                                    EntityChangedToken.EntityChangedAction.Created);
                _eventAggregator.GetEvent<OrganizationChanged>()
                                .Publish(token);

                return output;
            }
            else return null;
        }

        public OrganizationRole CreateNewOrganizationRole()
        {
            StringInputDialog creationDialog = new StringInputDialog();
            creationDialog.Title = "Crea nuovo Ruolo Organizzazione";

            if (creationDialog.ShowDialog() == true)
            {
                OrganizationRole output = new OrganizationRole();
                output.Description = "";
                output.Name = creationDialog.InputString;
                _lInstData.Execute(new InsertEntityCommand<LInstContext>(output));

                CreateMappingsForNewRole(output);
                return output;
            }

            return null;
        }

        public Person CreateNewPerson()
        {
            StringInputDialog addPersonDialog = new StringInputDialog
            {
                Title = "Creazione nuova Persona",
                Message = "Nome:"
            };

            if (addPersonDialog.ShowDialog() != true)
                return null;

            Person newPerson = new Person
            {
                Name = addPersonDialog.InputString
            };

            foreach (PersonRole prr in _lInstData.RunQuery(new PersonRolesQuery()))
            {
                PersonRoleMapping tempPRM = new PersonRoleMapping();
                tempPRM.RoleID = prr.ID;
                tempPRM.IsSelected = false;
                newPerson.RoleMappings.Add(tempPRM);
            }

            _lInstData.Execute(new InsertEntityCommand<LInstContext>(newPerson));

            return newPerson;
        }

        public PersonRole CreateNewPersonRole()
        {
            StringInputDialog addPersonRoleDialog = new StringInputDialog();
            addPersonRoleDialog.Title = "Creazione nuovo Ruolo Persona";
            addPersonRoleDialog.Message = "Nome:";

            if (addPersonRoleDialog.ShowDialog() != true)
                return null;

            PersonRole newRole = new PersonRole
            {
                Name = addPersonRoleDialog.InputString,
                Description = ""
            };

            using (LInstContext entities = _dbContextFactory.CreateDbContext(new string[] { }))
            {
                entities.PersonRoles.Add(newRole);

                foreach (Person per in entities.People)
                {
                    PersonRoleMapping newMapping = new PersonRoleMapping
                    {
                        Person = per,
                        IsSelected = false
                    };
                    newRole.RoleMappings.Add(newMapping);
                }

                entities.SaveChanges();
            }

            return newRole;
        }

        public User CreateNewUser()
        {
            Views.NewUserDialog newUserDialog = new Views.NewUserDialog();

            if (newUserDialog.ShowDialog() == true)
                return newUserDialog.NewUserInstance;
            else
                return null;
        }

        public UserRole CreateNewUserRole()
        {
            StringInputDialog addPersonDialog = new StringInputDialog
            {
                Title = "Creazione nuovo Ruolo Utente",
                Message = "Nome:"
            };

            if (addPersonDialog.ShowDialog() != true)
                return null;

            UserRole newRole = new UserRole
            {
                Name = addPersonDialog.InputString,
                Description = ""
            };

            _lInstData.Execute(new InsertEntityCommand<LInstContext>(newRole));

            foreach (User usr in _lInstData.RunQuery(new UsersQuery()))
            {
                UserRoleMapping newMap = new UserRoleMapping
                {
                    IsSelected = false,
                    UserRoleID = newRole.ID,
                    UserID = usr.ID
                };

                _lInstData.Execute(new InsertEntityCommand<LInstContext>(newMap));
            }

            return newRole;
        }

        /// <summary>
        /// Creates and inserts in the DB the mappings between a new OrganizationRole
        /// and all the existing organization
        /// </summary>
        /// <param name="newRole">The role for which will be built the mappings</param>
        internal void CreateMappingsForNewRole(OrganizationRole newRole)
        {
            using (LInstContext entities = _dbContextFactory.CreateDbContext(new string[] { }))
            {
                IEnumerable<Organization> _orgList = entities.Organizations.ToList();

                foreach (Organization org in _orgList)
                {
                    OrganizationRoleMapping tempMap = new OrganizationRoleMapping()
                    {
                        IsSelected = false,
                        OrganizationRoleID = newRole.ID
                    };

                    org.RoleMappings.Add(tempMap);
                }

                entities.SaveChanges();
            }
        }

        #endregion Methods
    }
}