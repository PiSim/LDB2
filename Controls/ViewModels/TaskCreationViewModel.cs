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
    internal class TaskCreationViewModel : BindableBase
    {
        private DBEntities _entities;
        private Views.TaskCreationDialog _parentView;

        internal TaskCreationViewModel(DBEntities entities,
                                    Views.TaskCreationDialog parentView) : base()
        {
            _entities = entities;
            _parentView = parentView;
        }
    }
}
