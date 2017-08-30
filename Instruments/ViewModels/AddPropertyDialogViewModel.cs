using DBManager;
using DBManager.EntityExtensions;
using DBManager.Services;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Instruments.ViewModels
{
    public class AddPropertyDialogViewModel : BindableBase
    {
        private DelegateCommand<Window> _cancel,
                                        _confirm;
        private MeasurableQuantity _selectedQuantity;

        public AddPropertyDialogViewModel()
        {
            _cancel = new DelegateCommand<Window>(
                dialog =>
                {
                    dialog.DialogResult = false;
                });

            _confirm = new DelegateCommand<Window>(
                dialog =>
                {
                    dialog.DialogResult = true;
                },
                dialog => _selectedQuantity != null);
        }

        public DelegateCommand<Window> CancelCommand
        {
            get { return _cancel; }
        }

        public DelegateCommand<Window> ConfirmCommand
        {
            get { return _confirm; }
        }

        public IEnumerable<MeasurableQuantity> QuantityList
        {
            get { return InstrumentService.GetMeasurableQuantities(); }
        }

        public MeasurableQuantity SelectedQuantity
        {
            get { return _selectedQuantity; }
            set
            {
                _selectedQuantity = value;
                _confirm.RaiseCanExecuteChanged();
            }
        }

    }
}
