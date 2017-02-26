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