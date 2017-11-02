using Controls.Views;
using DBManager;
using Infrastructure;
using Infrastructure.Events;
using Microsoft.Practices.Unity;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Admin
{
    public class AdminServiceProvider
    {
        private DBEntities _entities;
        private DBPrincipal _principal;
        private EventAggregator _eventAggregator;
        private IUnityContainer _container;

        public AdminServiceProvider(DBEntities entities,
                                    DBPrincipal principal,
                                    EventAggregator aggregator,
                                    IUnityContainer container)
        {
            _entities = entities;
            _eventAggregator = aggregator;
            _container = container;
            _principal = principal;

            _eventAggregator.GetEvent<UserCreationRequested>()
                            .Subscribe(() =>
                            {
                                NewUserRegistration();
                            });
        }

        public void AddOrganizationRole(string name)
        {
            OrganizationRole newRole = new OrganizationRole
            {
                Name = name,
                Description = ""
            };

            _entities.OrganizationRoles.Add(newRole);

            foreach (Organization org in _entities.Organizations)
            {
                OrganizationRoleMapping newMapping = new OrganizationRoleMapping
                {
                    Role = newRole,
                    Organization = org,
                    IsSelected = false
                };
                _entities.OrganizationRoleMappings.Add(newMapping);
            }

            _entities.SaveChanges();
        }

        public void AddPersonRole()
        {
            StringInputDialog addPersonRoleDialog = _container.Resolve<StringInputDialog>();
            addPersonRoleDialog.Title = "Creazione nuovo Ruolo Persona";
            addPersonRoleDialog.Message = "Nome:";

            if (addPersonRoleDialog.ShowDialog() != true)
                return;

            PersonRole newRole = new PersonRole
            {
                Name = addPersonRoleDialog.InputString,
                Description = ""
            };

            _entities.PersonRoles.Add(newRole);

            foreach (Person per in _entities.People)
            {
                PersonRoleMapping newMapping = new PersonRoleMapping
                {
                    Person = per,
                    IsSelected = false
                };
                newRole.RoleMappings.Add(newMapping);
            }

            _entities.SaveChanges();
        }

        public void BuildOrganizationRoles()
        {
            List<Organization> tempOrgList = new List<Organization>(_entities.Organizations);
            List<OrganizationRole> tempRoles = new List<OrganizationRole>(_entities.OrganizationRoles);
            foreach (Organization org in tempOrgList)
            {
                foreach (OrganizationRole rol in tempRoles)
                {
                    OrganizationRoleMapping temp = new OrganizationRoleMapping
                    {
                        Role = rol,
                        IsSelected = false
                    };
                    org.RoleMapping.Add(temp);
                }
            }

            _entities.SaveChanges();
        }

        public User NewUserRegistration()
        {
            Views.NewUserDialog newUserDialog = _container.Resolve<Views.NewUserDialog>();

            if (newUserDialog.ShowDialog() == true)
                return newUserDialog.NewUserInstance;

            else
                return null;
        }

    }

}


