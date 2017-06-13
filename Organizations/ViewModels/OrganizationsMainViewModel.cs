using DBManager;
using DBManager.Services;
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
        private DelegateCommand _createNewOrganization, 
                                _deleteOrganization;
        private EventAggregator _eventAggregator;
        private Organization _selectedOrganization;

        public OrganizationsMainViewModel(EventAggregator aggregator) : base()
        {
            _eventAggregator = aggregator;
            
            _eventAggregator.GetEvent<OrganizationListRefreshRequested>()
                            .Subscribe(() => RaisePropertyChanged("OrganizationList"));


            _createNewOrganization = new DelegateCommand(
                () =>
                {
                    _eventAggregator.GetEvent<OrganizationCreationRequested>()
                                    .Publish();
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
                _selectedOrganization.Load();

                RaisePropertyChanged("SelectedOrganization");
                RaisePropertyChanged("RoleList");
            }
        }

        public IEnumerable<Organization> OrganizationList
        {
            get
            {
                return OrganizationService.GetOrganizations();
            }
        }

        public IEnumerable<OrganizationRoleMapping> RoleList
        {
            get
            {
                if (_selectedOrganization == null)
                    return new List<OrganizationRoleMapping>();

                else
                    return _selectedOrganization.RoleMapping;
            }
        }
        
    }
}
