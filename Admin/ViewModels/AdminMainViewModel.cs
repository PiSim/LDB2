using DBManager;
using DBManager.EntityExtensions;
using DBManager.Services;
using Infrastructure;
using Infrastructure.Events;
using Infrastructure.Wrappers;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.ViewModels
{
    public class AdminMainViewModel : BindableBase
    {
        private DelegateCommand _newOrganizationRole, _newPersonRole, _newUserRole, _runMethod;
        private EventAggregator _eventAggregator;
        private IAdminService _adminService;
        private IDataService _dataService;
        private string _name;

        public AdminMainViewModel(EventAggregator eventAggregator,
                                    IAdminService adminService,
                                    IDataService dataService) : base()
        {
            _adminService = adminService;
            _dataService = dataService;
            _eventAggregator = eventAggregator;

            _newOrganizationRole = new DelegateCommand(
                () =>
                {
                    _adminService.CreateNewOrganization();
                });

            _newPersonRole = new DelegateCommand(
                () =>
                {
                    _adminService.CreateNewPersonRole();
                });

            _newUserRole = new DelegateCommand(
                () =>
                {
                    _adminService.CreateNewUserRole();
                });

            _runMethod = new DelegateCommand(
                () =>
                {
                    Mtd();
                        
                } );
        }

        public string AdminUserMainRegionName
        {
            get { return RegionNames.AdminUserMainRegion; }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
            }
        }

        public DelegateCommand AddPersonRoleCommand
        {
            get { return _newPersonRole; }
        }

        public string InstrumentTypeManagementRegionName
        {
            get { return RegionNames.InstrumentTypeManagementRegion; }
        }

        public string InstrumentUtilizationAreasRegionName
        {
            get { return RegionNames.InstrumentUtilizationAreasRegion; }
        }

        public string MeasurableQuantityManagementRegionName
        {
            get { return RegionNames.MeasurableQuantityManagementRegion; }
        }

        public DelegateCommand NewOrganizationRoleCommand
        {
            get { return _newOrganizationRole; }
        }

        public DelegateCommand NewUserRoleCommand
        {
            get { return _newUserRole; }
        }

        public string OrganizationRoleManagementRegionName
        {
            get { return RegionNames.OrganizationRoleManagementRegion; }
        }

        public IEnumerable<PersonRole> PersonRoleList
        {
            get { return _dataService.GetPersonRoles(); }
        }

        public string PeopleManagementRegionName => RegionNames.PeopleManagementRegion;

        public string PropertyRegionName => RegionNames.PropertyManagementRegion;

        public DelegateCommand RunMethodCommand
        {
            get { return _runMethod; }
        }

        public string UnitOfMeasurementManagementRegionName
        {
            get { return RegionNames.UnitOfMeasurementManagementRegion; }
        }

        private void Mtd()
        {
           
        }
    }
}
