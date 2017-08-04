using DBManager;
using DBManager.EntityExtensions;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.ViewModels
{
    public class MeasurableQuantityEditViewModel : BindableBase
    {
        private DelegateCommand _addMeasurementUnit;
        private MeasurableQuantity _measurableQuantityInstance;

        public MeasurableQuantityEditViewModel()
        {
            _addMeasurementUnit = new DelegateCommand(
                () =>
                {
                    Views.MeasurementUnitCreationDialog creationDialog = new Views.MeasurementUnitCreationDialog();
                    creationDialog.MeasurableQuantityInstance = _measurableQuantityInstance;
                    creationDialog.CanModifyQuantity = false;

                    if (creationDialog.ShowDialog() == true)
                        RaisePropertyChanged("UnitOfMeasurementList");
                });
        }

        public DelegateCommand AddMeasurementUnitCommand
        {
            get { return _addMeasurementUnit; }
        }

        public MeasurableQuantity MeasurableQuantityInstance
        {
            get { return _measurableQuantityInstance; }
            set
            {
                _measurableQuantityInstance = value;
                RaisePropertyChanged("MeasurementUnitList");
            }
        }

        public IEnumerable<MeasurementUnit> MeasurementUnitList
        {
            get { return _measurableQuantityInstance.GetMeasurementUnits(); }
        }
    }
}
