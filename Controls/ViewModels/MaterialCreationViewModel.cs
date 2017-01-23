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
        private DBEntities _entities;
        private DelegateCommand _confirmCreation;
        private string _aspect, _line, _recipe, _type;
        private Views.MaterialCreationDialog _parentDialog;

        public MaterialCreationViewModel(DBEntities entities, Views.MaterialCreationDialog parentDialog) 
            : base()
        {
            _entities = entities;
            _parentDialog = parentDialog;

            _confirmCreation = new DelegateCommand(
                () => {
                    Material temp = _entities.GetMaterial(_type,
                                                          _line,
                                                          _aspect,
                                                          _recipe);
                    _parentDialog.ValidatedMaterial = temp;
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
