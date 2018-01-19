using DBManager;
using DBManager.Services;
using Infrastructure;
using Infrastructure.Events;
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
    public class PropertiesMainViewModel : BindableBase
    {
        private DelegateCommand _newProperty;
        private EventAggregator _eventAggregator;
        private IAdminService _adminService;
        private IDataService _dataService;
        private Property _selectedProperty;

        public PropertiesMainViewModel(EventAggregator eventAggregator,
                                            IAdminService adminService,
                                            IDataService dataService)
        {
            _adminService = adminService;
            _dataService = dataService;
            _eventAggregator = eventAggregator;

            _newProperty = new DelegateCommand(
                () =>
                {
                    _adminService.CreateNewProperty();
                });
            
        }

        public IEnumerable<Property> PropertyList => _dataService.GetProperties();

        public string PropertyManagementEditRegionName
        {
            get { return RegionNames.PropertyManagementEditRegion; }
        }

        public DelegateCommand NewPropertyCommand
        {
            get { return _newProperty; }
        }

        public Property SelectedProperty
        {
            get { return _selectedProperty; }
            set
            {
                _selectedProperty = value;

                NavigationToken token = new NavigationToken(AdminViewNames.PropertyEditView,
                                                            _selectedProperty,
                                                            RegionNames.PropertyManagementEditRegion);

                _eventAggregator.GetEvent<NavigationRequested>()
                                .Publish(token);
            }
        }

    }
}
