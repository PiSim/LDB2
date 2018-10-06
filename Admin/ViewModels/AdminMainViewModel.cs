using Controls.Views;
using Infrastructure;
using LabDbContext;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.Generic;

namespace Admin.ViewModels
{
    public class AdminMainViewModel : BindableBase
    {
        #region Fields

        private IAdminService _adminService;
        private IDataService _dataService;
        private IEventAggregator _eventAggregator;
        private DelegateCommand _runMethod;

        #endregion Fields

        #region Constructors

        public AdminMainViewModel(IEventAggregator eventAggregator,
                                    IAdminService adminService,
                                    IDataService dataService) : base()
        {
            _adminService = adminService;
            _dataService = dataService;
            _eventAggregator = eventAggregator;

            ScriptList = new List<ScriptBase>()
            {
                new Scripts.BuildTestRecords(),
                new Scripts.BuildExternalTestRecordsScript(),
                new Scripts.BuildMethodVersionRequirementReferences(),
                new Scripts.BuildExternalReportMethodVariantMappingScript(),
                new Scripts.BuildSubMethodPositionScript(),
                new Scripts.RemoveMaterialDuplicatesScript()
            };

            NewOrganizationRoleCommand = new DelegateCommand(
                () =>
                {
                    _adminService.CreateNewOrganization();
                });

            AddPersonRoleCommand = new DelegateCommand(
                () =>
                {
                    _adminService.CreateNewPersonRole();
                });

            NewUserRoleCommand = new DelegateCommand(
                () =>
                {
                    _adminService.CreateNewUserRole();
                });

            _runMethod = new DelegateCommand(
                () =>
                {
                    SelectedScript.Run();
                });
        }

        #endregion Constructors

        #region Properties

        public DelegateCommand AddPersonRoleCommand { get; }
        public string AdminUserMainRegionName => RegionNames.AdminUserMainRegion;

        public string InstrumentTypeManagementRegionName => RegionNames.InstrumentTypeManagementRegion;

        public string InstrumentUtilizationAreasRegionName => RegionNames.InstrumentUtilizationAreasRegion;

        public string MeasurableQuantityManagementRegionName => RegionNames.MeasurableQuantityManagementRegion;

        public string Name { get; set; }

        public DelegateCommand NewOrganizationRoleCommand { get; }

        public DelegateCommand NewUserRoleCommand { get; }

        public string OrganizationRoleManagementRegionName => RegionNames.OrganizationRoleManagementRegion;

        public string PeopleManagementRegionName => RegionNames.PeopleManagementRegion;
        public IEnumerable<PersonRole> PersonRoleList => _dataService.GetPersonRoles();
        public string PropertyRegionName => RegionNames.PropertyManagementRegion;

        public DelegateCommand RunMethodCommand => _runMethod;

        public IEnumerable<ScriptBase> ScriptList { get; }

        public ScriptBase SelectedScript { get; set; }

        public string UnitOfMeasurementManagementRegionName => RegionNames.UnitOfMeasurementManagementRegion;

        #endregion Properties
    }
}