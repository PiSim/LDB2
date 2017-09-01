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

namespace Instruments.ViewModels
{
    public class InstrumentMeasurablePropertyEditViewModel : BindableBase
    {
        private bool _editMode;
        private DelegateCommand _cancelEdit,
                                _save,
                                _startEdit;
        private IEnumerable<MeasurementUnit> _umList;
        private InstrumentMeasurableProperty _measurablePropertyInstance;
        private MeasurementUnit _selectedUM;


        public InstrumentMeasurablePropertyEditViewModel()
        {
            _editMode = false;

            _cancelEdit = new DelegateCommand(
                () =>
                {
                    EditMode = false;
                });

            _save = new DelegateCommand(
                () =>
                {
                    _measurablePropertyInstance.Update();
                    EditMode = false;
                },
                () => EditMode);

            _startEdit = new DelegateCommand(
                () =>
                {
                    EditMode = true;
                },
                () => !EditMode);
        }

        public string CalibrationDueDate
        {
            get
            {
                if (_measurablePropertyInstance == null || _measurablePropertyInstance.CalibrationDue == null)
                    return "//";

                return _measurablePropertyInstance?.CalibrationDue.Value.ToShortDateString();
            }
        }

        public int ControlPeriod
        {
            get
            {
                if (_measurablePropertyInstance == null)
                    return 0;

                return _measurablePropertyInstance.ControlPeriod;
            }
            set
            {
                _measurablePropertyInstance.ControlPeriod = value;
                if (_measurablePropertyInstance.UpdateCalibrationDueDate())
                    RaisePropertyChanged("CalibrationDueDate");
            }
        }

        public bool EditMode
        {
            get { return _editMode; }
            set
            {
                _editMode = value;
                RaisePropertyChanged("EditMode");
                _save.RaiseCanExecuteChanged();
                _startEdit.RaiseCanExecuteChanged();
            }
        }

        public bool IsUnderControl
        {
            get
            {
                if (_measurablePropertyInstance == null)
                    return false;

                return _measurablePropertyInstance.IsUnderControl;
            }

            set
            {
                _measurablePropertyInstance.IsUnderControl = value;
                _measurablePropertyInstance.UpdateCalibrationDueDate();
                RaisePropertyChanged("CalibrationDueDate");
            }
        }

        public string LastCalibration
        {
            get
            {
                if (_measurablePropertyInstance == null)
                    return "Mai";

                else
                {
                    CalibrationReport _lastCalibration = _measurablePropertyInstance.GetLastCalibration();

                    if (_lastCalibration == null)
                        return "Mai";

                    else
                        return _lastCalibration.Date.ToShortDateString();
                }
            }
        }

        public InstrumentMeasurableProperty MeasurablePropertyInstance
        {
            get { return _measurablePropertyInstance; }
            set
            {
                _measurablePropertyInstance = value;
                EditMode = false;

                _umList = _measurablePropertyInstance?.MeasurableQuantity.GetMeasurementUnits();
                RaisePropertyChanged("UMList");

                if (_umList != null)
                    _selectedUM = _umList.FirstOrDefault(um => um.ID == _measurablePropertyInstance?.UnitID);
                else
                    _selectedUM = null;

                RaisePropertyChanged("CalibrationDueDate");
                RaisePropertyChanged("IsUnderControl");
                RaisePropertyChanged("LastCalibration");
                RaisePropertyChanged("MeasurablePropertyInstance");
                RaisePropertyChanged("SelectedUM");
            }
        }

        public DelegateCommand SaveCommand
        {
            get { return _save; }
        }

        public MeasurementUnit SelectedUM
        {
            get { return _selectedUM; }
            set
            {
                _selectedUM = value;
                if (_measurablePropertyInstance != null && _selectedUM != null)
                    _measurablePropertyInstance.UnitID = _selectedUM.ID;
            }
        }

        public DelegateCommand StartEditCommand
        {
            get { return _startEdit; }
        }

        public IEnumerable<MeasurementUnit> UMList
        {
            get { return _umList; }
        }
    }
}
