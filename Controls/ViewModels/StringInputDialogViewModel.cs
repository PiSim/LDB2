using Prism.Commands;
using Prism.Mvvm;

namespace Controls.ViewModels
{
    public class StringInputDialogViewModel : BindableBase
    {
        #region Fields

        private string _message;
        private Views.StringInputDialog _parentDialog;

        #endregion Fields

        #region Constructors

        public StringInputDialogViewModel(Views.StringInputDialog parentDialog) : base()
        {
            _parentDialog = parentDialog;
            _message = "Generic message";

            CancelCommand = new DelegateCommand(
                () =>
                {
                    _parentDialog.DialogResult = false;
                });

            ConfirmCommand = new DelegateCommand(
                () =>
                {
                    _parentDialog.DialogResult = true;
                });
        }

        #endregion Constructors

        #region Properties

        public DelegateCommand CancelCommand { get; }

        public DelegateCommand ConfirmCommand { get; }

        public string InputString { get; set; }

        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                RaisePropertyChanged("Message");
            }
        }

        #endregion Properties
    }
}