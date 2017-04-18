using DBManager;
using Infrastructure;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Security.Views
{
    /// <summary>
    /// Interaction logic for LoginDialog.xaml
    /// </summary>
    public partial class LoginDialog : Window
    {
        private AuthenticationService _authenticator;
        private IUnityContainer _container;
        private User _authenticatedUser;

        public LoginDialog(AuthenticationService authenticator,
                            IUnityContainer container)
        {
            _authenticator = authenticator;
            _container = container;
            InitializeComponent();
        }

        private void Abort_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _authenticatedUser = _authenticator.AuthenticateUser
                (UserNameTextBox.Text, PasswordBox.Password);

                DBPrincipal _currentPrincipal = new DBPrincipal();
                _currentPrincipal.Identity = new DBIdentity(_authenticatedUser);
                _container.RegisterInstance(typeof(DBPrincipal),
                                            _currentPrincipal);
                DialogResult = true;
            }
            
            catch (UnauthorizedAccessException)
            {
                PasswordBox.Clear();
                UserNameTextBox.Clear();
            }
        }

        public User AuthenticatedUser
        {
            get { return _authenticatedUser; }
        }
    }
}
