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
    public class MaterialCreationViewModel : BindableBase
    {
        private DelegateCommand _confirmCreation;
        private string _aspect, _line, _recipe, _type;
        private Views.MaterialCreationDialog _parentDialog;

        public MaterialCreationViewModel(Views.MaterialCreationDialog parentDialog) 
            : base()
        {
            _parentDialog = parentDialog;

            _confirmCreation = new DelegateCommand(
                () => {
                    _parentDialog.MaterialType = _type;
                    _parentDialog.MaterialLine = _line;
                    _parentDialog.MaterialAspect = _aspect;
                    _parentDialog.MaterialRecipe = _recipe;
                    
                    _parentDialog.DialogResult = true;
                },
                () => IsValidInput);
        }

        public string Aspect
        {
            get { return _aspect; }
            set { _aspect = value; }
        }

        public DelegateCommand ConfirmCreationCommand
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
