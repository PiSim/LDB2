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
using System.Windows;

namespace Instruments.ViewModels
{
    public class InstrumentMeasurablePropertyEditViewModel : BindableBase
    {
        private bool _editMode;
        private DelegateCommand _cancelEdit,
                                _save,
                                _startEdit;
        private IEnumerable<MeasurementUnit> _umList;
        private IEnumerable<Organization> _organizationList;
        private InstrumentMeasurableProperty _measurablePropertyInstance;
        private MeasurementUnit _selectedUM;


        public InstrumentMeasurablePropertyEditViewModel()
        {
            _editMode = false;
            _organizationList = OrganizationService.GetOrganizations(OrganizationRoleNames.CalibrationLab);

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

        public bool EditMode
        {
            get { return _editMode; }
            set
            {
                _editMode = value;
                RaisePropertyChanged("EditMode");
                RaisePropertyChanged("CanEditCalibrationParam");
                _save.RaiseCanExecuteChanged();
                _startEdit.RaiseCanExecuteChanged();
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
                RaisePropertyChanged("ControlPeriod");
                RaisePropertyChanged("IsUnderControl");
                RaisePropertyChanged("LastCalibration");
                RaisePropertyChanged("MeasurablePropertyInstance");
                RaisePropertyChanged("PropertyViewVisibility");
                RaisePropertyChanged("SelectedCalibrationLab");
                RaisePropertyChanged("SelectedUM");
            }
        }

        public Visibility PropertyViewVisibility
        {
            get
            {
                return (_measurablePropertyInstance != null) ? Visibility.Visible : Visibility.Hidden;
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
