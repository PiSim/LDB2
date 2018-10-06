using Infrastructure;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace LabDB2.Views
{
    /// <summary>
    /// Interaction logic for LoginDialog.xaml
    /// </summary>
    public partial class LoginDialog : Window
    {
        #region Fields

        private AuthenticationService _authenticator;

        #endregion Fields

        #region Constructors

        public LoginDialog(AuthenticationService authenticator)
        {
            _authenticator = authenticator;
            InitializeComponent();
            UserNameTextBox.Text = Properties.Settings.Default.LastLogUser;

            if (!string.IsNullOrWhiteSpace(UserNameTextBox.Text))
                PasswordBox.Focus();
        }

        #endregion Constructors

        #region Properties

        public DBPrincipal AuthenticatedPrincipal { get; private set; }

        public LabDbContext.User AuthenticatedUser { get; private set; }

        public string UserName
        {
            get => UserNameTextBox.Text;
            set => UserNameTextBox.Text = value;
        }

        #endregion Properties

        #region Methods

        private void Abort_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void Confirm_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AuthenticatedUser = _authenticator.AuthenticateUser
                (UserNameTextBox.Text, PasswordBox.Password);

                AuthenticatedPrincipal = new DBPrincipal();
                AuthenticatedPrincipal.Identity = new DBIdentity(AuthenticatedUser);
                DialogResult = true;
            }
            catch (UnauthorizedAccessException)
            {
                PasswordBox.Clear();
                UserNameTextBox.Clear();
            }
        }

        private void Grid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                Confirm_Click(sender, e);
        }

        #endregion Methods
    }
}