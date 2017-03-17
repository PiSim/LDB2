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
    public class ColorPickerDialogViewModel : BindableBase
    {
        private Colour _selectedColour;
        private DBEntities _entities;
        private DelegateCommand _cancel, _confirm;
        private Views.ColorPickerDialog _parentView;

        public ColorPickerDialogViewModel(DBEntities entities, Views.ColorPickerDialog parent) : base()
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
                    _parentView.ColourInstance = SelectedColour;
                    _parentView.DialogResult = true;
                });
        }

        public DelegateCommand CancelCommand
        {
            get { return _cancel; }
        }

        public List<Colour> ColourList
        {
            get { return new List<Colour>(_entities.Colours.OrderBy(clr => clr.Name)); }
        }

        public DelegateCommand ConfirmCommand
        {
            get { return _confirm; }
        }

        public Colour SelectedColour
        {
            get { return _selectedColour; }
            set { _selectedColour = value; }
        }
    }
}
