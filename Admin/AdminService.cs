using Controls.Views;
using DBManager;
using DBManager.EntityExtensions;
using Infrastructure;
using Infrastructure.Events;
using Microsoft.Practices.Unity;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Admin
{
    public class AdminService : IAdminService
    {
        private DBPrincipal _principal;
        private EventAggregator _eventAggregator;
        private IDataService _dataService;
        private IUnityContainer _container;

        public AdminService(DBPrincipal principal,
                                    EventAggregator aggregator,
                                    IDataService dataService,
                                    IUnityContainer container)
        {
            _eventAggregator = aggregator;
            _container = container;
            _dataService = dataService;
            _principal = principal;
        }

        public void AddOrganizationRole(string name)
        {
            using (DBEntities entities = new DBEntities())
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
                        Role = newRole,
                        Organization = org,
                        IsSelected = false
                    };
                    entities.OrganizationRoleMappings.Add(newMapping);
                }

                entities.SaveChanges();
            }
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

            foreach (PersonRole prr in _dataService.GetPersonRoles())
            {
                PersonRoleMapping tempPRM = new PersonRoleMapping();
                tempPRM.roleID = prr.ID;
                tempPRM.IsSelected = false;
                newPerson.RoleMappings.Add(tempPRM);
            }

            newPerson.Create();

            return newPerson;
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

            newRole.Create();

            foreach (User usr in _dataService.GetUsers())
            {
                UserRoleMapping newMap = new UserRoleMapping
                {
                    IsSelected = false,
                    RoleID = newRole.ID,
                    UserID = usr.ID
                };

                newMap.Create();
            }

            return newRole;
        }

        public PersonRole CreateNewPersonRole()
        {
            StringInputDialog addPersonRoleDialog = _container.Resolve<StringInputDialog>();
            addPersonRoleDialog.Title = "Creazione nuovo Ruolo Persona";
            addPersonRoleDialog.Message = "Nome:";

            if (addPersonRoleDialog.ShowDialog() != true)
                return null;

            PersonRole newRole = new PersonRole
            {
                Name = addPersonRoleDialog.InputString,
                Description = ""
            };

            using (DBEntities entities = new DBEntities())
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
            Views.NewUserDialog newUserDialog = _container.Resolve<Views.NewUserDialog>();

            if (newUserDialog.ShowDialog() == true)
                return newUserDialog.NewUserInstance;

            else
                return null;
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

                output.Create();
                return output;
            }

            return null;
        }

        public MeasurableQuantity CreateNewMeasurableQuantity()
        {
            StringInputDialog creationDialog = new StringInputDialog
            {
                Title = "Crea nuova quantità misurabile"
            };

            if (creationDialog.ShowDialog() == true)
            {
                MeasurableQuantity output = new MeasurableQuantity()
                {
                    Name = creationDialog.InputString
                };

                output.Create();
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
                    Category = "",
                    Name = creationDialog.InputString
                };
                foreach (OrganizationRole orr in _dataService.GetOrganizationRoles())
                {
                    OrganizationRoleMapping tempORM = new OrganizationRoleMapping
                    {
                        IsSelected = false,
                        RoleID = orr.ID
                    };

                    output.RoleMapping.Add(tempORM);
                }

                output.Create();

                EntityChangedToken token = new EntityChangedToken(output,
                                                                    EntityChangedToken.EntityChangedAction.Created);
                _eventAggregator.GetEvent<OrganizationChanged>()
                                .Publish(token);

                return output;
            }
            else return null;
        }


        /// <summary>
        /// Creates and inserts in the DB the mappings between a new OrganizationRole
        /// and all the existing organization
        /// </summary>
        /// <param name="newRole">The role for which will be built the mappings</param>
        internal void CreateMappingsForNewRole(OrganizationRole newRole)
        {
            using (DBEntities entities = new DBEntities())
            {
                IEnumerable<Organization> _orgList = entities.Organizations.ToList();

                foreach (Organization org in _orgList)
                {
                    OrganizationRoleMapping tempMap = new OrganizationRoleMapping()
                    {
                        IsSelected = false,
                        RoleID = newRole.ID
                    };

                    org.RoleMapping.Add(tempMap);
                }

                entities.SaveChanges();
            }
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
                output.Create();

                CreateMappingsForNewRole(output);
                return output;
            }

            return null;
        }

        public Property CreateNewProperty()
        {
            StringInputDialog creationDialog = new StringInputDialog();
            creationDialog.Title = "Crea nuova Proprietà";

            if (creationDialog.ShowDialog() == true)
            {
                Property output = new Property();
                output.Name = creationDialog.InputString;
                output.Create();
                
                return output;
            }

            return null;
        }
    }

}


