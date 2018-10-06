using Controls.Views;
using Infrastructure;
using Infrastructure.Events;
using LabDbContext;
using LabDbContext.EntityExtensions;
using LabDbContext.Services;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.Generic;

namespace Admin.ViewModels
{
    public class MeasurableQuantityMainViewModel : BindableBase
    {
        #region Fields

        private IAdminService _adminService;
        private IDataService _dataService;
        private IEventAggregator _eventAggregator;
        private MeasurableQuantity _selectedMeasurableQuantity;

        #endregion Fields

        #region Constructors

        public MeasurableQuantityMainViewModel(IEventAggregator eventAggregator,
                                                IAdminService adminService,
                                                IDataService dataService) : base()
        {
            _adminService = adminService;
            _dataService = dataService;
            _eventAggregator = eventAggregator;

            DeleteQuantityCommand = new DelegateCommand(
                () =>
                {
                    _selectedMeasurableQuantity.Delete();
                });

            NewMeasurableQuantityCommand = new DelegateCommand(
                () =>
                {
                    _adminService.CreateNewMeasurableQuantity();
                });

            _eventAggregator.GetEvent<MeasurableQuantityCreated>()
                            .Subscribe(
                            quantity =>
                            {
                                RaisePropertyChanged("MeasurableQuantityList");
                            });
        }

        #endregion Constructors

        #region Properties

        public DelegateCommand DeleteQuantityCommand { get; }
        public IEnumerable<MeasurableQuantity> MeasurableQuantityList => _dataService.GetMeasurableQuantities();
        public string MeasurableQuantityManagementEditRegionName => RegionNames.MeasurableQuantityManagementEditRegion;
        public DelegateCommand NewMeasurableQuantityCommand { get; }

        public MeasurableQuantity SelectedMeasurableQuantity
        {
            get { return _selectedMeasurableQuantity; }
            set
            {
                _selectedMeasurableQuantity = value;

                NavigationToken token = new NavigationToken(AdminViewNames.MeasurableQuantityEdit,
                                                            _selectedMeasurableQuantity,
                                                            RegionNames.MeasurableQuantityManagementEditRegion);

                _eventAggregator.GetEvent<NavigationRequested>()
                                .Publish(token);
            }
        }

        #endregion Properties
    }
}