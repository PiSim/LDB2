using DataAccess;
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

        private IDataService<LabDbEntities> _labDbData;
        private MeasurableQuantity _selectedQuantity;

        #endregion Fields

        #region Constructors

        public AddPropertyDialogViewModel(IDataService<LabDbEntities> labDbData)
        {
            _labDbData = labDbData;

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

        //public IEnumerable<MeasurableQuantity> QuantityList => _labDbData.RunQuery(new MeasurableQuantity;

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