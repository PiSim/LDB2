using DBManager;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Controls.ViewModels
{
    public class BatchPickerDialogViewModel : BindableBase
    {
        private DelegateCommand<Window> _cancel, _confirm;
        private string _number;
        
        public BatchPickerDialogViewModel() : base()
        {            
            _cancel = new DelegateCommand<Window>(
                parentDialog => 
                {                    
                    parentDialog.DialogResult = false;
                });
                
            _confirm = new DelegateCommand<Window>(
                parentDialog => 
                {
                    parentDialog.DialogResult = true;
                });
        }
        
        public DelegateCommand<Window> CancelCommand
        {
            get { return _cancel; }
        }
        
        public DelegateCommand<Window> ConfirmCommand
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
