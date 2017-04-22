using Controls.Views;
using DBManager;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;


public class AdminServiceProvider
{
    private DBEntities _entities;
    private IUnityContainer _container;

    public AdminServiceProvider(DBEntities entities,
                                IUnityContainer container)
    {
        _entities = entities;
        _container = container;
    }

    public void AddOrganizationRole(string name)
    {
        OrganizationRole newRole = new OrganizationRole();
        newRole.Name = name;
        newRole.Description = "";

        _entities.OrganizationRoles.Add(newRole);

        foreach (Organization org in _entities.Organizations)
        {
            OrganizationRoleMapping newMapping = new OrganizationRoleMapping();
            newMapping.Role = newRole;
            newMapping.Organization = org;
            newMapping.IsSelected = false;
            _entities.OrganizationRoleMappings.Add(newMapping);
        }

        _entities.SaveChanges();
    }

    public void AddUserRole(string name)
    {
        UserRole newRole = new UserRole();
        newRole.Name = name;
        newRole.Description = "";

        _entities.UserRoles.Add(newRole);

        foreach (User usr in _entities.Users)
        {
            UserRoleMapping newMapping = new UserRoleMapping();
            newMapping.UserRole = newRole;
            newMapping.User = usr;
            newMapping.IsSelected = false;
            _entities.UserRoleMappings.Add(newMapping);
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

        PersonRole newRole = new PersonRole();
        newRole.Name = addPersonRoleDialog.InputString;
        newRole.Description = "";

        _entities.PersonRoles.Add(newRole);

        foreach (Person per in _entities.People)
        {
            PersonRoleMapping newMapping = new PersonRoleMapping();
            newMapping.Person = per;
            newMapping.IsSelected = false;
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
                OrganizationRoleMapping temp = new OrganizationRoleMapping();
                temp.Role = rol;
                temp.IsSelected = false;
                org.RoleMapping.Add(temp);
            }
        }

        _entities.SaveChanges();
    }

}