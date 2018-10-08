using DataAccess;
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

        private IDataService<LabDbEntities> _labDbData;
        private IEventAggregator _eventAggregator;

        #endregion Fields

        #region Constructors

        public MeasurementUnitMainViewModel(IEventAggregator eventAggregator,
                                            IDataService<LabDbEntities> labDbData)
        {
            _eventAggregator = eventAggregator;
            _labDbData = labDbData;

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

        //public IEnumerable<MeasurementUnit> UnitOfMeasurementList => _labDbData.RunQuery(new);

        #endregion Properties
    }
}