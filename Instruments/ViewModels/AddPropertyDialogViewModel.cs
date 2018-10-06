using LabDbContext;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Windows;

namespace Instruments.ViewModels
{
    public class AddPropertyDialogViewModel : BindableBase
    {
        #region Fields

        private IDataService _dataService;
        private MeasurableQuantity _selectedQuantity;

        #endregion Fields

        #region Constructors

        public AddPropertyDialogViewModel(IDataService dataService)
        {
            _dataService = dataService;

            CancelCommand = new DelegateCommand<Window>(
                dialog =>
                {
                    dialog.DialogResult = false;
                });

            ConfirmCommand = new DelegateCommand<Window>(
                dialog =>
                {
                    dialog.DialogResult = true;
                },
                dialog => _selectedQuantity != null);
        }

        #endregion Constructors

        #region Properties

        public DelegateCommand<Window> CancelCommand { get; }

        public DelegateCommand<Window> ConfirmCommand { get; }

        public IEnumerable<MeasurableQuantity> QuantityList => _dataService.GetMeasurableQuantities();

        public MeasurableQuantity SelectedQuantity
        {
            get { return _selectedQuantity; }
            set
            {
                _selectedQuantity = value;
                ConfirmCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion Properties
    }
}