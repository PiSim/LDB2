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
    internal class BatchPickerViewModel : BindableBase
    {
        private DBEntities _entities;
        private DelegateCommand _cancel, _confirm;
        private string _number;
        private Views.BatchPickerDialog _parentDialog;
        
        internal BatchPickerViewModel(DBEntities entities,
                                    Views.BatchPickerDialog parentDialog) : base()
        {
            _entities = entities;
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
