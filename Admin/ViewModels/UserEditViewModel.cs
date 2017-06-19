using DBManager;
using DBManager.EntityExtensions;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Admin.ViewModels
{
    public class UserEditViewModel : BindableBase
    {
        private bool _editMode;
        private DelegateCommand _save, _startEdit;
        private IEnumerable<Person> _peopleList;
        private User _userInstance;

        public UserEditViewModel() : base()
        {
            _editMode = false;

            _save = new DelegateCommand(
                () =>
                {
                    _userInstance.Update();
                    EditMode = false;
                },
                () => _editMode = true);

            _startEdit = new DelegateCommand(
                () => 
                {
                    _editMode = true;
                },
                () => _editMode = false && _userInstance != null);
        }


        public bool EditMode
        {
            get { return _editMode; }
            set
            {
                _editMode = value;

                RaisePropertyChanged("EditMode");
                _save.RaiseCanExecuteChanged();
                _startEdit.RaiseCanExecuteChanged();
            }
        }

        public DelegateCommand SaveCommand
        {
            get { return _save; }
        }

        public DelegateCommand StartEditCommand
        {
            get { return _startEdit; }
        }
    }
}
