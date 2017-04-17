using DBManager;
using Infrastructure;
using Microsoft.Practices.Unity;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organizations
{
    public class OrganizationServiceProvider : IOrganizationServiceProvider
    {
        private DBEntities _entities;
        private EventAggregator _aggregator;
        private IUnityContainer _container;

        public OrganizationServiceProvider(DBEntities entities,
                                            EventAggregator aggregator,
                                            IUnityContainer container)
        {
            _entities = entities;
            _aggregator = aggregator;
            _container = container;
        }
        
        public Organization CreateNewOrganization()
        {
            Views.OrganizationCreationDialog creationDialog = _container.Resolve<Views.OrganizationCreationDialog>();
            if (creationDialog.ShowDialog() == true)
            {
                Organization output = new Organization();
                output.Category = "";
                output.Name = creationDialog.OrganizationName;
                foreach (OrganizationRole orr in _entities.OrganizationRoles)
                {
                    OrganizationRoleMapping tempORM = new OrganizationRoleMapping();
                    tempORM.IsSelected = false;
                    tempORM.Role = orr;

                    output.RoleMapping.Add(tempORM);
                }

                _entities.Organizations.Add(output);
                _entities.SaveChanges();
                return output;
            }
            else return null;
        }
    }
}
