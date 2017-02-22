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
        private Views.BatchPickerDialog _parentDialog;
        
        internal BatchPickerViewModel(DBEntities entities,
                                    Views.BatchPickerDialog parentDialog) : base()
        {
            _entities = entities;
            _parentDialog = parentDialog;
        }
        
        
        
    }
}
