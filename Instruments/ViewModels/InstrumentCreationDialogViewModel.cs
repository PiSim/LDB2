using DBManager;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instruments.ViewModels
{

    internal class InstrumentCreationDialogViewModel : BindableBase
    {
        private bool _isUnderControl;
        private DBEntities _entities;
        private DelegateCommand _cancel, _confirm;
        private InstrumentType _selectedType;
        private int _controlPeriod;
        private Organization _manufacturer;
        private string _code, _model, _serial;
        private Views.InstrumentCreationDialog _parentDialog;

        internal InstrumentCreationDialogViewModel(Views.InstrumentCreationDialog parentDialog,
                                                    DBEntities entities) : base()
        {
            _entities = entities;
            _parentDialog = parentDialog;

            _cancel = new DelegateCommand(
                () =>
                {
                    _parentDialog.DialogResult = false;
                });

            _confirm = new DelegateCommand(
                () =>
                {
                    Instrument newInstrument = new Instrument();
                    newInstrument.Code = _code;
                    newInstrument.Description = "";
                    newInstrument.ControlPeriod = (sbyte)_controlPeriod;
                    newInstrument.InstrumentType = _selectedType;
                    newInstrument.IsUnderControl = IsUnderControl;

                    if (_isUnderControl)
                        newInstrument.CalibrationDueDate = DateTime.Now.Date;

                    newInstrument.Manufacturer = SelectedManufacturer;
                    newInstrument.Model = Model;
                    newInstrument.SerialNumber = _serial;

                    _entities.Instruments.Add(newInstrument);
                    _entities.SaveChanges();

                    _parentDialog.InstrumentInstance = newInstrument;
                    _parentDialog.DialogResult = true;
                },
                () => IsValidInput);
        }

        public DelegateCommand CancelCommand
        {
            get { return _cancel; }
        }

        public string Code
        {
            get { return _code; }
            set { _code = value; }
        }

        public DelegateCommand ConfirmCommand
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

        public List<Organization> ManufacturerList
        {
            get 
            { 

                OrganizationRole _manufacturerRole = _entities.OrganizationRoles.First(rol => rol.Name == "MANUF");
                return new List<Organization>(_entities.Organizations.Where(org => org.RoleMapping
                                                                    .Any(orm => orm.Role.ID == _manufacturerRole.ID && orm.IsSelected == true))
                                                                    .OrderBy(oor => oor.Name)); 
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

        public List<InstrumentType> TypeList
        {
            get { return new List<InstrumentType>(_entities.InstrumentTypes.OrderBy(inty => inty.Name)); }
        }
    }
}
