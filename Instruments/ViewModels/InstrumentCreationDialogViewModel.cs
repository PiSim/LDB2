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
        private DBEntities _entities;
        private DelegateCommand _cancel, _confirm;
        private InstrumentType _selectedType;
        private string _code;
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
                    newInstrument.ControlPeriod = 0;
                    newInstrument.InstrumentType = _selectedType;
                    newInstrument.IsUnderControl = false;
                    newInstrument.Model = "";
                    newInstrument.SerialNumber = "";

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

        private bool IsValidInput
        {
            get { return true; }
        }

        public InstrumentType SelectedType
        {
            get { return _selectedType; }
            set { _selectedType = value; }
        }

        public List<InstrumentType> TypeList
        {
            get { return new List<InstrumentType>(_entities.InstrumentTypes); }
        }
    }
}
