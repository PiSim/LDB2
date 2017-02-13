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
        private Colour _selectedColor;
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
                    _parentView.DialogResult = false;
                });

            _confirm = new DelegateCommand(
                () =>
                {
                    _parentView.ColourInstance = SelectedColor;
                    _parentView.DialogResult = true;
                });
        }

        public DelegateCommand CancelCommand
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

        public Colour SelectedColor
        {
            get { return _selectedColor; }
            set { _selectedColor = value; }
        }
    }
}
