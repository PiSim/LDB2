using DBManager;
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
    internal class OrganizationsMainViewModel : BindableBase
    {
        private DBEntities _entities;
        private EventAggregator _eventAggregator;
        private Organization _selectedOrganization;

        internal OrganizationsMainViewModel(DBEntities entities,
                                            EventAggregator aggregator) : base()
        {
            _entities = entities;
            _eventAggregator = aggregator;

            _eventAggregator.GetEvent<CommitRequested>()
                            .Subscribe(() => _entities.SaveChanges());
        }

        public Organization SelectedOrganization
        {
            get { return _selectedOrganization; }
            set
            {
                _selectedOrganization = value;
                OnPropertyChanged("SelectedOrganization");
                OnPropertyChanged("RoleList");
            }
        }

        public ObservableCollection<Organization> OrganizationList
        {
            get
            {
                return new ObservableCollection<Organization>
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
