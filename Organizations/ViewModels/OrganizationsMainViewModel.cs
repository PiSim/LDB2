using DBManager;
using Infrastructure;
using Infrastructure.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Organizations.ViewModels
{
    public class OrganizationsMainViewModel : BindableBase
    {
        private DBEntities _entities;
        private DelegateCommand _createNewOrganization;
        private EventAggregator _eventAggregator;
        private IOrganizationServiceProvider _organizationServiceProvider;
        private Organization _selectedOrganization;

        public OrganizationsMainViewModel(DBEntities entities,
                                            EventAggregator aggregator,
                                            IOrganizationServiceProvider organizationServiceProvider) : base()
        {
            _entities = entities;
            _eventAggregator = aggregator;
            _organizationServiceProvider = organizationServiceProvider;

            _eventAggregator.GetEvent<CommitRequested>()
                            .Subscribe(() => _entities.SaveChanges());

            _createNewOrganization = new DelegateCommand(
                () =>
                {
                    Organization tempOrg = _organizationServiceProvider.CreateNewOrganization();
                    if (tempOrg != null)
                        RaisePropertyChanged("OrganizationList");
                });
        }

        public DelegateCommand CreateNewOrganizationCommand
        {
            get { return _createNewOrganization; }
        }

        public Organization SelectedOrganization
        {
            get { return _selectedOrganization; }
            set
            {
                _selectedOrganization = value;
                RaisePropertyChanged("SelectedOrganization");
                RaisePropertyChanged("RoleList");
            }
        }

        public List<Organization> OrganizationList
        {
            get
            {
                return new List<Organization>
                    (_entities.Organizations.OrderBy(org => org.Name));
            }
        }

        public List<OrganizationRoleMapping> RoleList
        {
            get
            {
                if (_selectedOrganization == null)
                    return new List<OrganizationRoleMapping>();

                else
                    return new List<OrganizationRoleMapping>
                        (_selectedOrganization.RoleMapping);
            }
        }
        
    }
}
