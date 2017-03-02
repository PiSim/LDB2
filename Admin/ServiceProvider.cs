using DBManager;
using System;
using System.Collections.Generic;
using System.Linq;


public class ServiceProvider
{
    private DBEntities _entities;
    
    public ServiceProvider(DBEntities entities)
    {
        _entities = entities;
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