using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controls.ViewModels
{
    public class NewPODialogViewModel : BindableBase 
    {
        private DelegateCommand _cancel, _confirm;
        private Views.NewPODialog _parentDialog;

        public NewPODialogViewModel(Views.NewPODialog parentDialog) : base()
        {
            _parentDialog = parentDialog;

            _cancel = new DelegateCommand(
                () =>
                {

                });

            _confirm = new DelegateCommand(
                () =>
                {

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
    }
}
