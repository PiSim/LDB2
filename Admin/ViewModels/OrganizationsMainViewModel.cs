using Controls.Views;
using DataAccessCore;
using Infrastructure;
using Infrastructure.Events;
using Infrastructure.Queries;
using LInst;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Linq;

namespace Admin.ViewModels
{
    public class OrganizationsMainViewModel : BindableBase
    {
        #region Fields

        private IAdminService _adminService;
        private IEventAggregator _eventAggregator;
        private IDataService<LInstContext> _lInstData;
        private Organization _selectedOrganization;

        #endregion Fields

        #region Constructors

        public OrganizationsMainViewModel(IDataService<LInstContext> lInstData,
                                            IEventAggregator aggregator,
                                            IAdminService adminService) : base()
        {
            _lInstData = lInstData;
            _adminService = adminService;
            _eventAggregator = aggregator;

            #region EventSubscriptions

            _eventAggregator.GetEvent<OrganizationChanged>()
                            .Subscribe(ect => RaisePropertyChanged("OrganizationList"),
                            ThreadOption.PublisherThread,
                            false,
                            ect => ect.Action != EntityChangedToken.EntityChangedAction.Updated);

            #endregion EventSubscriptions

            CreateNewOrganizationCommand = new DelegateCommand(
                () =>
                {
                    _adminService.CreateNewOrganization();
                });

            CreateNewOrganizationRoleCommand = new DelegateCommand(
                () =>
                {
                    _adminService.CreateNewOrganizationRole();
                });
        }

        #endregion Constructors

        #region Properties

        public DelegateCommand CreateNewOrganizationCommand { get; }

        public DelegateCommand CreateNewOrganizationRoleCommand { get; }

        public string OrganizationEditRegionName => RegionNames.OrganizationEditRegion;

        public IEnumerable<Organization> OrganizationList => _lInstData.RunQuery(new OrganizationsQuery())
                                                                        .ToList();

        public IEnumerable<OrganizationRoleMapping> RoleList
        {
            get
            {
                if (_selectedOrganization == null)
                    return new List<OrganizationRoleMapping>();
                else
                    return _selectedOrganization.RoleMappings;
            }
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

        #endregion Properties
    }
}