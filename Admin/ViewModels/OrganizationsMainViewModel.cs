using Controls.Views;
using DataAccess;
using Infrastructure;
using Infrastructure.Events;
using Infrastructure.Queries;
using LabDbContext;
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
        private IDataService<LabDbEntities> _labDbData;
        private Organization _selectedOrganization;

        #endregion Fields

        #region Constructors

        public OrganizationsMainViewModel(IDataService<LabDbEntities> labDbData,
                                            IEventAggregator aggregator,
                                            IAdminService adminService) : base()
        {
            _labDbData = labDbData;
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

        public IEnumerable<Organization> OrganizationList => _labDbData.RunQuery(new OrganizationsQuery())
                                                                        .ToList();

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