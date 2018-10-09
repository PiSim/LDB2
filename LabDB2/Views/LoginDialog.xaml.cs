using DataAccess;
using Infrastructure;
using LabDB2.Commands;
using LabDbContext;
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

        private IAuthenticationService _authenticator;
        private IDataService<LabDbEntities> _labDbData;

        #endregion Fields

        #region Constructors

        public LoginDialog(IAuthenticationService authenticator,
                            IDataService<LabDbEntities> labDbData)
        {
            _authenticator = authenticator;
            _labDbData = labDbData;
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
                _labDbData.Execute(new AuthenticateUserCommand(new LabDbContext.User()
                                                                    {
                                                                        UserName = UserNameTextBox.Text,
                                                                        HashedPassword = _authenticator.CalculateHash(PasswordBox.Password, UserNameTextBox.Text)
                                                                    }, 
                                                                    OnAuthenticationHandler));
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

        public void OnAuthenticationHandler(LabDbContext.User authenticatedUser)
        {
            if (authenticatedUser == null)
                throw new UnauthorizedAccessException();
            AuthenticatedPrincipal = new DBPrincipal();
            AuthenticatedPrincipal.Identity = new DBIdentity(authenticatedUser);
            DialogResult = true;
        }

        #endregion Methods
    }
}