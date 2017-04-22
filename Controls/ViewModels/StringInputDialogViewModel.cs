using DBManager;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controls.ViewModels
{
    public class StringInputDialogViewModel : BindableBase
    {
        private DBEntities _entities;
        private DelegateCommand _cancel, _confirm;
        private string _message, _payload;
        private Views.StringInputDialog _parentDialog;

        public StringInputDialogViewModel(Views.StringInputDialog parentDialog) : base()
        {
            _parentDialog = parentDialog;
            _message = "Generic message";

            _cancel = new DelegateCommand(
                () =>
                {
                    _parentDialog.DialogResult = false;
                });

            _confirm = new DelegateCommand(
                () =>
                {
                    _parentDialog.DialogResult = true;
                });
        }

        public DelegateCommand CancelCommand
        {
            get { return _cancel; }
        }

        public DelegateCommand ConfirmCommand
        {
            get { return _confirm; }
        }
        
        public string InputString
        {
            get { return _payload; }
            set
            {
                _payload = value;
            }
        }

        public string Message
        {
            get { return _message; }
            set
            {
                _message = value;
                OnPropertyChanged("Message");
            }
        }
    }
}
