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
    internal class ColorPickerViewModel : BindableBase
    {
        private DBEntities _entities;
        private DelegateCommand _cancel, _confirm;
        private Views.ColorPickerDialog _parentView;

        internal ColorPickerViewModel(DBEntities entities, Views.ColorPickerDialog parent) : base()
        {
            _entities = entities;
            _parentView = parent;

            _cancel = new DelegateCommand(
                () =>
                {
                    
                });

            _confirm = new DelegateCommand(
                () =>
                {

                });
        }

        public DelegateCommand CandcelCommand
        {
            get { return _cancel; }
        }

        public List<Colour> ColourList
        {
            get { return new List<Colour>(_entities.Colours); }
        }

        public DelegateCommand ConfirmCommand
        {
            get { return _confirm; }
        }
    }
}
