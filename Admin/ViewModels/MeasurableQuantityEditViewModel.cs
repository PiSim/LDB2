using LabDbContext;
using LabDbContext.EntityExtensions;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.Generic;

namespace Admin.ViewModels
{
    public class MeasurableQuantityEditViewModel : BindableBase
    {
        #region Fields

        private MeasurableQuantity _measurableQuantityInstance;

        #endregion Fields

        #region Constructors

        public MeasurableQuantityEditViewModel()
        {
            AddMeasurementUnitCommand = new DelegateCommand(
                () =>
                {
                    Views.MeasurementUnitCreationDialog creationDialog = new Views.MeasurementUnitCreationDialog();
                    creationDialog.MeasurableQuantityInstance = _measurableQuantityInstance;
                    creationDialog.CanModifyQuantity = false;

                    if (creationDialog.ShowDialog() == true)
                        RaisePropertyChanged("UnitOfMeasurementList");
                });
        }

        #endregion Constructors

        #region Properties

        public DelegateCommand AddMeasurementUnitCommand { get; }

        public MeasurableQuantity MeasurableQuantityInstance
        {
            get { return _measurableQuantityInstance; }
            set
            {
                _measurableQuantityInstance = value;
                RaisePropertyChanged("MeasurementUnitList");
            }
        }

        public IEnumerable<MeasurementUnit> MeasurementUnitList => _measurableQuantityInstance.GetMeasurementUnits();

        #endregion Properties
    }
}