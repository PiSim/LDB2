using DBManager;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Materials.ViewModels
{
    public class BatchPickerDialogViewModel : BindableBase
    {
        private DelegateCommand _cancel, _confirm;
        private string _number;
        private Views.BatchPickerDialog _parentDialog;
        
        public BatchPickerDialogViewModel(Views.BatchPickerDialog parentDialog) : base()
        {
            _parentDialog = parentDialog;
            
            _cancel = new DelegateCommand(
                () => 
                {                    
                    _parentDialog.DialogResult = false;
                });
                
            _confirm = new DelegateCommand(
                () => 
                {
                    _parentDialog.BatchNumber = Number;
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
        
        public string Number
        {
            get { return _number; }
            set 
            {
                _number = value;
            }
        }        
        
    }
}
