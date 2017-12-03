using DBManager;
using DBManager.EntityExtensions;
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
    public class MeasurableQuantityMainViewModel : BindableBase
    {
        private DelegateCommand _deleteQuantity,
                                _newQuantity;
        private EventAggregator _eventAggregator;
        private IAdminService _adminService;
        private IDataService _dataService;
        private MeasurableQuantity _selectedMeasurableQuantity;

        public MeasurableQuantityMainViewModel(EventAggregator eventAggregator,
                                                IAdminService adminService,
                                                IDataService dataService) : base()
        {
            _adminService = adminService;
            _dataService = dataService;
            _eventAggregator = eventAggregator;

            _deleteQuantity = new DelegateCommand(
                () =>
                {
                    _selectedMeasurableQuantity.Delete();
                });

            _newQuantity = new DelegateCommand(
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

        public string MeasurableQuantityManagementEditRegionName
        {
            get { return RegionNames.MeasurableQuantityManagementEditRegion; }
        }

        public IEnumerable<MeasurableQuantity> MeasurableQuantityList
        {
            get { return _dataService.GetMeasurableQuantities(); }
        }

        public DelegateCommand DeleteQuantityCommand
        {
            get { return _deleteQuantity; }
        }


        public DelegateCommand NewMeasurableQuantityCommand
        {
            get { return _newQuantity; }
        }

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
    }
}
