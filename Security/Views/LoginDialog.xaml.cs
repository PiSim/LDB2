using DBManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private User _authenticatedUser;

        public LoginDialog(AuthenticationService authenticator)
        {
            _authenticator = authenticator;
            InitializeComponent();
        }

        private void Abort_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            _authenticatedUser = _authenticator.AuthenticateUser
                (UserNameTextBox.Text, PasswordBox.Password);

            if (_authenticatedUser != null)
                DialogResult = true;
        }

        public User AuthenticatedUser
        {
            get { return _authenticatedUser; }
        }
    }
}
