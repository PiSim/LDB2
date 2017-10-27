using DBManager;
using DBManager.EntityExtensions;
using DBManager.Services;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace Instruments.ViewModels
{

    internal class InstrumentCreationDialogViewModel : BindableBase, INotifyDataErrorInfo
    {
        private bool _isUnderControl;
        private DelegateCommand<Window> _cancel, _confirm;
        private readonly Dictionary<string, ICollection<string>> _validationErrors = new Dictionary<string, ICollection<string>>();
        private IEnumerable<Organization> _calibrationLabList;
        private Instrument _instrumentInstance;
        private InstrumentType _selectedType;
        private InstrumentUtilizationArea _selectedArea;
        private int _controlPeriod;
        private Organization _manufacturer,
                            _selectedCalibrationLab;
        private string _code, 
                        _model,
                        _notes,
                        _serial;

        public InstrumentCreationDialogViewModel() : base()
        {
            _controlPeriod = 12;
            _notes = "";
            _model = "";
            _serial = "";
            _calibrationLabList = OrganizationService.GetOrganizations(OrganizationRoleNames.CalibrationLab);

            _cancel = new DelegateCommand<Window>(
                parent =>
                {
                    parent.DialogResult = false;
                });

            _confirm = new DelegateCommand<Window>(
                parent =>
                {
                    _instrumentInstance = new Instrument();
                    _instrumentInstance.Code = _code;
                    _instrumentInstance.Description = _notes;
                    _instrumentInstance.InstrumentTypeID = _selectedType.ID;
                    _instrumentInstance.IsInService = true;
                    _instrumentInstance.IsUnderControl = _isUnderControl;
                    _instrumentInstance.CalibrationInterval = _controlPeriod;
                    _instrumentInstance.UtilizationAreaID = _selectedArea.ID;
                    _instrumentInstance.CalibrationDueDate = (_isUnderControl) ? DateTime.Now : (DateTime?)null;
                    _instrumentInstance.CalibrationResponsibleID = _selectedCalibrationLab?.ID;
                    _instrumentInstance.manufacturerID = SelectedManufacturer.ID;
                    _instrumentInstance.Model = Model;
                    _instrumentInstance.SerialNumber = _serial;
                    
                    foreach (MeasurableQuantity meq in _selectedType.GetAssociatedMeasurableQuantities())
                    {
                        InstrumentMeasurableProperty tempIMP = new InstrumentMeasurableProperty()
                        {
                            MeasurableQuantityID = meq.ID,
                            UnitID = meq.UnitsOfMeasurement.First().ID
                        };
                        
                        _instrumentInstance.InstrumentMeasurableProperties.Add(tempIMP);
                    }

                    _instrumentInstance.Create();


                    parent.DialogResult = true;
                },
                parent => !HasErrors);

            SelectedCalibrationLab = null;
        }


        #region INotifyDataErrorInfo interface elements

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName)
                || !_validationErrors.ContainsKey(propertyName))
                return null;

            return _validationErrors[propertyName];
        }

        public bool HasErrors
        {
            get { return _validationErrors.Count > 0; }
        }

        private void RaiseErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            _confirm.RaiseCanExecuteChanged();
        }

        #endregion

        public DelegateCommand<Window> CancelCommand
        {
            get { return _cancel; }
        }

        public IEnumerable<Organization> CalibrationLabList
        {
            get { return _calibrationLabList; }
        }

        public string Code
        {
            get { return _code; }
            set
            {
                _code = value;

                if (string.IsNullOrEmpty(_code))
                    InstrumentInstance = null;

                else
                    InstrumentInstance = InstrumentService.GetInstrument(_code);

                if (InstrumentInstance == null &&
                    _validationErrors.ContainsKey("Code"))
                {
                    _validationErrors.Remove("Code");
                    RaiseErrorsChanged("Code");
                }

                else if (InstrumentInstance != null)
                {
                    _validationErrors["Code"] = new List<string>() { "Lo strumento " + _code + " esiste già" };
                    RaiseErrorsChanged("Code");
                }
            }
        }

        public DelegateCommand<Window> ConfirmCommand
        {
            get { return _confirm; }
        }

        public int ControlPeriod
        {
            get { return _controlPeriod; }
            set
            {
                _controlPeriod = value;
            }
        }

        public Instrument InstrumentInstance
        {
            get { return _instrumentInstance; }
            private set
            {
                _instrumentInstance = value;
            }
        }

        public bool IsUnderControl
        {
            get { return _isUnderControl; }
            set
            {
                _isUnderControl = value;
                RaisePropertyChanged("IsUnderControl");
                if (value)
                    SelectedCalibrationLab = _calibrationLabList.First(lab => lab.Name == "Vulcaflex");
                else
                    SelectedCalibrationLab = null;
            }
        }

        private bool IsValidInput
        {
            get { return true; }
        }

        public IEnumerable<Organization> ManufacturerList
        {
            get 
            {
                return OrganizationService.GetOrganizations(OrganizationRoleNames.Manufacturer);                
            }
        }

        public string Model
        {
            get { return _model; }
            set 
            {
                _model = value;
            }
        }

        public string Notes
        {
            get { return _notes; }
            set
            {
                _notes = value;
            }
        }

        public Organization SelectedCalibrationLab
        {
            get
            {
                return _selectedCalibrationLab;
            }

            set
            {
                _selectedCalibrationLab = value;
                if (_selectedCalibrationLab == null && IsUnderControl)
                    _validationErrors["SelectedCalibrationLab"] = new List<string>() { "Selezionare un laboratorio" };

                else
                    _validationErrors.Remove("SelectedCalibrationLab");

                RaiseErrorsChanged("SelectedCalibrationLab");
                RaisePropertyChanged("SelectedCalibrationLab");
            }
        }

        public Organization SelectedManufacturer
        {
            get { return _manufacturer;}
            set
            {
                _manufacturer = value;
            }
        }

        public InstrumentType SelectedType
        {
            get { return _selectedType; }
            set { _selectedType = value; }
        }

        public InstrumentUtilizationArea SelectedArea
        {
            get { return _selectedArea; }
            set
            {
                _selectedArea = value;
            }
        }

        public string SerialNumber
        {
            get { return _serial; }
            set
            {
                _serial = value;
            }
        }

        public IEnumerable<InstrumentType> TypeList
        {
            get { return InstrumentService.GetInstrumentTypes(); }
        }

        public IEnumerable<InstrumentUtilizationArea> AreaList
        {
            get { return InstrumentService.GetUtilizationAreas(); }
        }

    }
}
