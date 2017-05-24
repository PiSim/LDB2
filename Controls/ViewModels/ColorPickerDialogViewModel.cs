using DBManager;
using DBManager.Services;
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
    public class ColorPickerDialogViewModel : BindableBase
    {
        private Colour _selectedColour;
        private DelegateCommand<Window> _cancel, _confirm;
        
        public ColorPickerDialogViewModel() : base()
        {

            _cancel = new DelegateCommand<Window>(
                parent =>
                {
                    parent.DialogResult = false;
                });

            _confirm = new DelegateCommand<Window>(
                parent =>
                {
                    parent.DialogResult = true;
                });
        }

        public DelegateCommand<Window> CancelCommand
        {
            get { return _cancel; }
        }

        public IEnumerable<Colour> ColourList
        {
            get { return MaterialService.GetColours(); }
        }

        public DelegateCommand<Window> ConfirmCommand
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
