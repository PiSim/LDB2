using DBManager;
using DBManager.Services;
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
    public class MeasurementUnitMainViewModel : BindableBase
    {
        private DelegateCommand _newUnit;
        private EventAggregator _eventAggregator;
        private MeasurementUnit _selectedUnit;

        public MeasurementUnitMainViewModel(EventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;

            _newUnit = new DelegateCommand(
                () =>
                {
                    Views.MeasurementUnitCreationDialog creationDialog = new Views.MeasurementUnitCreationDialog();

                    if (creationDialog.ShowDialog() == true)
                        RaisePropertyChanged("UnitOfMeasurementList");
                });
        }

        public DelegateCommand NewUnitCommand
        {
            get { return _newUnit; }
        }

        public MeasurementUnit SelectedUnit
        {
            get { return _selectedUnit; }
            set
            {
                _selectedUnit = value;
            }
        }

        public IEnumerable<MeasurementUnit> UnitOfMeasurementList
        {
            get { return InstrumentService.GetMeasurementUnits(); }
        }
    }
}
