using DBManager;
using DBManager.Services;
using Prism.Commands;
using Prism.Mvvm;
using Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Admin.ViewModels
{
    public class NewUserDialogViewModel : BindableBase
    {
        AuthenticationService _authenticator; 
        private DelegateCommand<Window> _cancel, _confirm;
        private IDataService _dataService;
        private Person _selectedPerson;
        private string _userName;
        private User _userInstance;

        public NewUserDialogViewModel(AuthenticationService authenticator,
                                        IDataService dataService) : base()
        {
            _authenticator = authenticator;
            _dataService = dataService;

            _cancel = new DelegateCommand<Window>(
                parentDialog =>
                {
                    parentDialog.DialogResult = false;
                });

            _confirm = new DelegateCommand<Window>(
                parentDialog =>
                {
                    
                    if ((parentDialog as Views.NewUserDialog).PasswordBox1.Password 
                        != (parentDialog as Views.NewUserDialog).PasswordBox2.Password)
                    {
                        (parentDialog as Views.NewUserDialog).PasswordBox1.Clear();
                        (parentDialog as Views.NewUserDialog).PasswordBox1.Clear();
                    }

                    else
                    {
                        _userInstance = _authenticator.CreateNewUser(_selectedPerson,
                                                                    _userName,
                                                                    (parentDialog as Views.NewUserDialog).PasswordBox1.Password);
                        parentDialog.DialogResult = true;
                    }
                }, 
                parentDialog => IsValidInput);
        }

        public DelegateCommand<Window> CancelCommand
        {
            get { return _cancel; }
        }

        public DelegateCommand<Window> ConfirmCommand
        {
            get { return _confirm; }
        }

        public bool IsValidInput
        {
            get
            {
                return _userName != null && 
                        _selectedPerson != null;
            }
        }

        public IEnumerable<Person> PeopleList => _dataService.GetPeople();

        public Person SelectedPerson
        {
            get { return _selectedPerson; }
            set
            {
                _selectedPerson = value;
                _confirm.RaiseCanExecuteChanged();
            }
        }

        public User UserInstance
        {
            get { return _userInstance; }
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
