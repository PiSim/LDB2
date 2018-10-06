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
    public class PropertiesMainViewModel : BindableBase
    {
        #region Fields

        private IAdminService _adminService;
        private IEventAggregator _eventAggregator;
        private IDataService<LabDbEntities> _labDbData;
        private Property _selectedProperty;

        #endregion Fields

        #region Constructors

        public PropertiesMainViewModel(IDataService<LabDbEntities> labDbdata, 
                                        IEventAggregator eventAggregator,
                                            IAdminService adminService)
        {
            _labDbData = labDbdata;
            _adminService = adminService;
            _eventAggregator = eventAggregator;

            NewPropertyCommand = new DelegateCommand(
                () =>
                {
                    _adminService.CreateNewProperty();
                });
        }

        #endregion Constructors

        #region Properties

        public DelegateCommand NewPropertyCommand { get; }
        public IEnumerable<Property> PropertyList => _labDbData.RunQuery(new PropertiesQuery()).ToList();

        public string PropertyManagementEditRegionName => RegionNames.PropertyManagementEditRegion;

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

        #endregion Properties
    }
}