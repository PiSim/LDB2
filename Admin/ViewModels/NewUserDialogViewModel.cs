using DBManager;
using Prism.Commands;
using Prism.Mvvm;
using Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Admin.ViewModels
{
    public class NewUserDialogViewModel : BindableBase
    {
        AuthenticationService _authenticator;
        private DBEntities _entities;
        private DelegateCommand _cancel, _confirm;
        private Views.NewUserDialog _parentDialog;
        private Person _selectedPerson;
        private string _password, _passwordConfirmation, _userName;

        public NewUserDialogViewModel(AuthenticationService authenticator,
                                        DBEntities entities,
                                        Views.NewUserDialog parentDialog) : base()
        {
            _authenticator = authenticator;
            _entities = entities;
            _parentDialog = parentDialog;

            _cancel = new DelegateCommand(
                () =>
                {
                    _parentDialog.DialogResult = false;
                });

            _confirm = new DelegateCommand(
                () =>
                {
                    if (_password != _passwordConfirmation)
                    {
                        Password = null;
                        PasswordConfirmation = null;
                    }

                    else
                    {
                        User output = _authenticator.CreateNewUser(_selectedPerson,
                                                                    _userName,
                                                                    _password);
                        _parentDialog.NewUserInstance = output;
                        _parentDialog.DialogResult = true;
                    }
                }, 
                () => IsValidInput);
        }

        public DelegateCommand CancelCommand
        {
            get { return _cancel; }
        }

        public DelegateCommand ConfirmCommand
        {
            get { return _confirm; }
        }

        public bool IsValidInput
        {
            get
            {
                return _userName != null && 
                        _password != null &&
                        _passwordConfirmation != null &&
                        _selectedPerson != null;
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                _password = value;
                OnPropertyChanged("Password");
                _confirm.RaiseCanExecuteChanged();
            }
        }

        public string PasswordConfirmation
        {
            get { return _passwordConfirmation; }
            set
            {
                _passwordConfirmation = value;
                OnPropertyChanged("PasswordConfirmation");
                _confirm.RaiseCanExecuteChanged();
            }
        }

        public List<Person> PeopleList
        {
            get { return new List<Person>(_entities.People); }
        }

        public Person SelectedPerson
        {
            get { return _selectedPerson; }
            set
            {
                _selectedPerson = value;
                _confirm.RaiseCanExecuteChanged();
            }
        }

        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                _confirm.RaiseCanExecuteChanged();
            }
        }
    }
}
