using DataAccessCore;
using Infrastructure;
using Infrastructure.Queries;
using LInst;
using Prism.Commands;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Admin.ViewModels
{
    public class NewUserDialogViewModel : BindableBase
    {
        #region Fields

        private IAuthenticationService _authenticator;
        private IDataService<LInstContext> _lInstData;
        private Person _selectedPerson;
        private string _userName;

        #endregion Fields

        #region Constructors

        public NewUserDialogViewModel(IAuthenticationService authenticator,
                                        IDataService<LInstContext> lInstData) : base()
        {
            _authenticator = authenticator;
            _lInstData = lInstData;

            CancelCommand = new DelegateCommand<Window>(
                parentDialog =>
                {
                    parentDialog.DialogResult = false;
                });

            ConfirmCommand = new DelegateCommand<Window>(
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
                        UserInstance = _authenticator.CreateNewUser(_selectedPerson,
                                                                    _userName,
                                                                    (parentDialog as Views.NewUserDialog).PasswordBox1.Password);
                        parentDialog.DialogResult = true;
                    }
                },
                parentDialog => IsValidInput);
        }

        #endregion Constructors

        #region Properties

        public DelegateCommand<Window> CancelCommand { get; }

        public DelegateCommand<Window> ConfirmCommand { get; }

        public bool IsValidInput => _userName != null &&
                        _selectedPerson != null;

        public IEnumerable<Person> PeopleList => _lInstData.RunQuery(new PeopleQuery()).ToList();

        public Person SelectedPerson
        {
            get { return _selectedPerson; }
            set
            {
                _selectedPerson = value;
                ConfirmCommand.RaiseCanExecuteChanged();
            }
        }

        public User UserInstance { get; private set; }

        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                ConfirmCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion Properties
    }
}