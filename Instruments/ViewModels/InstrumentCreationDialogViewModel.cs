using DBManager;
using DBManager.EntityExtensions;
using DBManager.Services;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Instruments.ViewModels
{

    internal class InstrumentCreationDialogViewModel : BindableBase
    {
        private bool _isUnderControl;
        private DelegateCommand<Window> _cancel, _confirm;
        private Instrument _instrumentInstance;
        private InstrumentType _selectedType;
        private int _controlPeriod;
        private Organization _manufacturer;
        private string _code, _model, _serial;

        public InstrumentCreationDialogViewModel() : base()
        {

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
                    _instrumentInstance.Description = "";
                    _instrumentInstance.ControlPeriod = (sbyte)_controlPeriod;
                    _instrumentInstance.InstrumentTypeID = _selectedType.ID;
                    _instrumentInstance.IsUnderControl = IsUnderControl;

                    if (_isUnderControl)
                        _instrumentInstance.CalibrationDueDate = DateTime.Now.Date;

                    _instrumentInstance.manufacturerID = SelectedManufacturer.ID;
                    _instrumentInstance.Model = Model;
                    _instrumentInstance.SerialNumber = _serial;
                    
                    foreach (MeasurableQuantity meq in _selectedType.GetAssociatedMeasurableQuantities())
                    {
                        InstrumentMeasurableProperty tempIMP = new InstrumentMeasurableProperty()
                        {
                            IsUnderControl = _isUnderControl,
                            MeasurableQuantityID = meq.ID,
                            UnitID = meq.UnitsOfMeasurement.First().ID
                        };
                        
                        _instrumentInstance.instrument_measurable_property.Add(tempIMP);
                    }

                    _instrumentInstance.Create();


                    parent.DialogResult = true;
                },
                parent => IsValidInput);
        }

        public DelegateCommand<Window> CancelCommand
        {
            get { return _cancel; }
        }

        public string Code
        {
            get { return _code; }
            set { _code = value; }
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
        }

        public bool IsUnderControl
        {
            get { return _isUnderControl; }
            set
            {
                _isUnderControl = value;
                RaisePropertyChanged("IsUnderControl");
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
    }
}
