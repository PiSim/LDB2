using DBManager;
using DBManager.EntityExtensions;
using DBManager.Services;
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
        private IEnumerable<UserRoleMapping> _roleList;
        private User _userInstance;

        public UserEditViewModel() : base()
        {
            _editMode = false;
            _peopleList = PeopleService.GetPeople();

            _save = new DelegateCommand(
                () =>
                {
                    _userInstance.Update();
                    _roleList.Update();
                    EditMode = false;
                },
                () => _editMode == true);

            _startEdit = new DelegateCommand(
                () => 
                {
                    EditMode = true;
                },
                () => _editMode == false && _userInstance != null);
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

        public IEnumerable<Person> PeopleList
        {
            get { return _peopleList; }
        }

        public IEnumerable<UserRoleMapping> RoleList
        {
            get { return _roleList; }

            private set
            {
                _roleList = value;
                RaisePropertyChanged("RoleList");
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

        public User UserInstance
        {
            get { return _userInstance; }
            set
            {
                _userInstance = value;
                RoleList = _userInstance.GetRoles();
                EditMode = false;
            }
        }
        
    }
}
