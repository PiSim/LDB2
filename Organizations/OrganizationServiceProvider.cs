using Controls.Views;
using DBManager;
using Infrastructure;
using Infrastructure.Events;
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

            _aggregator.GetEvent<OrganizationCreationRequested>()
                        .Subscribe(() =>
                        {
                            CreateNewOrganization();
                        });
        }
        
        public Organization CreateNewOrganization()
        {
            StringInputDialog creationDialog = _container.Resolve<StringInputDialog>();
            creationDialog.Title = "Crea nuovo Ente";

            if (creationDialog.ShowDialog() == true)
            {
                Organization output = new Organization();
                output.Category = "";
                output.Name = creationDialog.InputString;
                foreach (OrganizationRole orr in _entities.OrganizationRoles)
                {
                    OrganizationRoleMapping tempORM = new OrganizationRoleMapping();
                    tempORM.IsSelected = false;
                    tempORM.Role = orr;

                    output.RoleMapping.Add(tempORM);
                }

                _entities.Organizations.Add(output);
                _entities.SaveChanges();

                _aggregator.GetEvent<OrganizationListRefreshRequested>()
                            .Publish();

                return output;
            }
            else return null;
        }
    }
}
