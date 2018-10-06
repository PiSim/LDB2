using Prism.Commands;
using Prism.Mvvm;
using System.Windows;

namespace Controls.ViewModels
{
    public class BatchPickerDialogViewModel : BindableBase
    {
        #region Constructors

        public BatchPickerDialogViewModel() : base()
        {
            CancelCommand = new DelegateCommand<Window>(
                parentDialog =>
                {
                    parentDialog.DialogResult = false;
                });

            ConfirmCommand = new DelegateCommand<Window>(
                parentDialog =>
                {
                    parentDialog.DialogResult = true;
                });
        }

        #endregion Constructors

        #region Properties

        public DelegateCommand<Window> CancelCommand { get; }

        public DelegateCommand<Window> ConfirmCommand { get; }

        public string Number { get; set; }

        #endregion Properties
    }
}