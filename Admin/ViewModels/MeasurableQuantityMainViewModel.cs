using Controls.Views;
using DataAccess;
using Infrastructure;
using Infrastructure.Commands;
using Infrastructure.Events;
using LabDbContext;
using LabDbContext.EntityExtensions;
using LabDbContext.Services;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;

namespace Admin.ViewModels
{
    [Obsolete]
    public class MeasurableQuantityMainViewModel : BindableBase
    {
        #region Fields

        private IAdminService _adminService;
        private IEventAggregator _eventAggregator;
        private MeasurableQuantity _selectedMeasurableQuantity;
        private IDataService<LabDbEntities> _labDbData;

        #endregion Fields

        #region Constructors

        public MeasurableQuantityMainViewModel(IEventAggregator eventAggregator,
                                                IAdminService adminService,
                                                IDataService<LabDbEntities> labDbData) : base()
        {
            _adminService = adminService;
            _labDbData = labDbData;
            _eventAggregator = eventAggregator;

            DeleteQuantityCommand = new DelegateCommand(
                () =>
                {
                    _labDbData.Execute(new DeleteEntityCommand(_selectedMeasurableQuantity));
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