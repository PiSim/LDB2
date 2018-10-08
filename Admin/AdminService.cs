using Admin.Queries;
using Controls.Views;
using DataAccess;
using Infrastructure;
using Infrastructure.Commands;
using Infrastructure.Events;
using LabDbContext;
using LabDbContext.EntityExtensions;
using Prism.Events;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace Admin
{
    public class AdminService : IAdminService
    {
        #region Fields
        
        private IDbContextFactory<LabDbEntities> _dbContextFactory;
        private IEventAggregator _eventAggregator;
        private IDataService<LabDbEntities> _labDbData;

        #endregion Fields

        #region Constructors

        public AdminService(IDbContextFactory<LabDbEntities> dbContextFactory,
                            IEventAggregator aggregator,
                            IDataService<LabDbEntities> labDbData)
        {
            _eventAggregator = aggregator;
            _dbContextFactory = dbContextFactory;
            _labDbData = labDbData;
        }

        #endregion Constructors

        #region Methods

        public void AddOrganizationRole(string name)
        {
            using (LabDbEntities entities = _dbContextFactory.Create())
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

                _labDbData.Execute(new InsertEntityCommand(output));
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
                foreach (OrganizationRole orr in _labDbData.RunQuery(new OrganizationRolesQuery()))
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

        public OrganizationRole CreateNewOrganizationRole()
        {
            StringInputDialog creationDialog = new StringInputDialog();
            creationDialog.Title = "Crea nuovo Ruolo Organizzazione";

            if (creationDialog.ShowDialog() == true)
            {
                OrganizationRole output = new OrganizationRole();
                output.Description = "";
                output.Name = creationDialog.InputString;
                _labDbData.Execute(new InsertEntityCommand(output));

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

            foreach (PersonRole prr in _labDbData.RunQuery(new PersonRolesQuery()))
            {
                PersonRoleMapping tempPRM = new PersonRoleMapping();
                tempPRM.roleID = prr.ID;
                tempPRM.IsSelected = false;
                newPerson.RoleMappings.Add(tempPRM);
            }

            _labDbData.Execute(new InsertEntityCommand(newPerson));

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

            using (LabDbEntities entities = _dbContextFactory.Create())
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

        public Property CreateNewProperty()
        {
            StringInputDialog creationDialog = new StringInputDialog();
            creationDialog.Title = "Crea nuova Proprietà";

            if (creationDialog.ShowDialog() == true)
            {
                Property output = new Property();
                output.Name = creationDialog.InputString;
                _labDbData.Execute(new InsertEntityCommand(output));

                return output;
            }

            return null;
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

            _labDbData.Execute(new InsertEntityCommand(newRole));

            foreach (User usr in _labDbData.RunQuery(new UsersQuery()))
            {
                UserRoleMapping newMap = new UserRoleMapping
                {
                    IsSelected = false,
                    RoleID = newRole.ID,
                    UserID = usr.ID
                };

                _labDbData.Execute(new InsertEntityCommand(newMap));
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
            using (LabDbEntities entities = _dbContextFactory.Create())
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

        #endregion Methods
    }
}