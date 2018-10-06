using LabDbContext;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.Generic;

namespace Admin.ViewModels
{
    public class MeasurementUnitMainViewModel : BindableBase
    {
        #region Fields

        private IDataService _dataService;
        private IEventAggregator _eventAggregator;

        #endregion Fields

        #region Constructors

        public MeasurementUnitMainViewModel(IEventAggregator eventAggregator,
                                            IDataService dataService)
        {
            _eventAggregator = eventAggregator;
            _dataService = dataService;

            NewUnitCommand = new DelegateCommand(
                () =>
                {
                    Views.MeasurementUnitCreationDialog creationDialog = new Views.MeasurementUnitCreationDialog();

                    if (creationDialog.ShowDialog() == true)
                        RaisePropertyChanged("UnitOfMeasurementList");
                });
        }

        #endregion Constructors

        #region Properties

        public DelegateCommand NewUnitCommand { get; }

        public MeasurementUnit SelectedUnit { get; set; }

        public IEnumerable<MeasurementUnit> UnitOfMeasurementList => _dataService.GetMeasurementUnits();

        #endregion Properties
    }
}