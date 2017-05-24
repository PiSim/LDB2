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
    public class MaterialCreationDialogViewModel : BindableBase
    {
        private DelegateCommand<Window> _confirmCreation;
        private string _aspect, _line, _recipe, _type;

        public MaterialCreationDialogViewModel() 
            : base()
        {
            _confirmCreation = new DelegateCommand<Window>(
                _parentDialog => {
                    _parentDialog.DialogResult = true;
                },
                _parentDialog => IsValidInput);
        }

        public string Aspect
        {
            get { return _aspect; }
            set { _aspect = value; }
        }

        public DelegateCommand<Window> ConfirmCreationCommand
        {
            get { return _confirmCreation; }
        }


        private bool IsValidInput
        {
            get
            {
                return true;
            }
        }

        public string Line
        {
            get { return _line; }
            set { _line = value; }
        }

        public string Recipe
        {
            get { return _recipe; }
            set { _recipe = value; }
        }

        public string Type
        {
            get { return _type; }
            set { _type = value; }
        }
    }
}
