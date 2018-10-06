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
    public class InstrumentTypeMainViewModel : BindableBase
    {
        #region Fields

        private IAdminService _adminService;
        private IEventAggregator _eventAggregator;
        private IDataService<LabDbEntities> _labDbData;
        private InstrumentType _selectedInstrumentType;

        #endregion Fields

        #region Constructors

        public InstrumentTypeMainViewModel(IEventAggregator eventAggregator,
                                            IAdminService adminService,
                                            IDataService<LabDbEntities> labDbData)
        {
            _adminService = adminService;
            _labDbData = labDbData;
            _eventAggregator = eventAggregator;

            NewInstrumentTypeCommand = new DelegateCommand(
                () =>
                {
                    _adminService.CreateNewInstrumentType();
                });

            _eventAggregator.GetEvent<InstrumentTypeCreated>()
                            .Subscribe(
                            instrumentType =>
                            {
                                RaisePropertyChanged("InstrumentTypeList");
                            });
        }

        #endregion Constructors

        #region Properties

        public IEnumerable<InstrumentType> InstrumentTypeList => _labDbData.RunQuery(new InstrumentTypesQuery()).ToList();

        public string InstrumentTypeManagementEditRegionName => RegionNames.InstrumentTypeManagementEditRegion;

        public DelegateCommand NewInstrumentTypeCommand { get; }

        public InstrumentType SelectedInstrumentType
        {
            get { return _selectedInstrumentType; }
            set
            {
                _selectedInstrumentType = value;

                NavigationToken token = new NavigationToken(AdminViewNames.InstrumentTypeEditView,
                                                            _selectedInstrumentType,
                                                            RegionNames.InstrumentTypeManagementEditRegion);

                _eventAggregator.GetEvent<NavigationRequested>()
                                .Publish(token);
            }
        }

        #endregion Properties
    }
}