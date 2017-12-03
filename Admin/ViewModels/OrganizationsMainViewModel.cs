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

namespace Admin.ViewModels
{
    public class OrganizationsMainViewModel : BindableBase
    {
        private DelegateCommand _createNewOrganization,
                                _createNewOrganizationRole,
#pragma warning disable CS0169 // Il campo 'OrganizationsMainViewModel._deleteOrganization' non viene mai usato
                                _deleteOrganization;
#pragma warning restore CS0169 // Il campo 'OrganizationsMainViewModel._deleteOrganization' non viene mai usato
        private EventAggregator _eventAggregator;
        private IAdminService _adminService;
        private IDataService _dataService;
        private Organization _selectedOrganization;

        public OrganizationsMainViewModel(EventAggregator aggregator,
                                            IAdminService adminService,
                                            IDataService dataService) : base()
        {
            _adminService = adminService;
            _dataService = dataService;
            _eventAggregator = aggregator;

            #region EventSubscriptions

            _eventAggregator.GetEvent<OrganizationChanged>()
                            .Subscribe(ect => RaisePropertyChanged("OrganizationList"));

            #endregion
            
            _createNewOrganization = new DelegateCommand(
                () =>
                {
                    _adminService.CreateNewOrganization();
                });

            _createNewOrganizationRole = new DelegateCommand(
                () =>
                {
                    _adminService.CreateNewOrganizationRole();
                });
        }

        public DelegateCommand CreateNewOrganizationCommand
        {
            get { return _createNewOrganization; }
        }

        public DelegateCommand CreateNewOrganizationRoleCommand
        {
            get { return _createNewOrganizationRole; }
        }

        public Organization SelectedOrganization
        {
            get { return _selectedOrganization; }
            set
            {
                _selectedOrganization = value;
                RaisePropertyChanged("SelectedOrganization");

                NavigationToken token = new NavigationToken(OrganizationViewNames.OrganizationEditView,
                                                            _selectedOrganization,
                                                            RegionNames.OrganizationEditRegion);
                _eventAggregator.GetEvent<NavigationRequested>()
                                .Publish(token);
            }
        }

        public string OrganizationEditRegionName
        {
            get { return RegionNames.OrganizationEditRegion; }
        }

        public IEnumerable<Organization> OrganizationList => _dataService.GetOrganizations();

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
