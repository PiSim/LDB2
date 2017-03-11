using DBManager;
using Security;
using Microsoft.Practices.Prism.Mvvm;
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

namespace Admin.Views
{
    /// <summary>
    /// Interaction logic for NewUserDialog.xaml
    /// </summary>
    public partial class NewUserDialog : Window
    {
        private User _newUserInstance;

        public NewUserDialog(AuthenticationService authenticator,
                            DBEntities entities)
        {
            DataContext = new ViewModels.NewUserDialogViewModel(authenticator,
                                                                entities,
                                                                this);
            InitializeComponent();
        }

        public User NewUserInstance
        {
            get { return _newUserInstance; }
            set { _newUserInstance = value; }
        }

        private void PasswordBox2_PasswordChanged(object sender, RoutedEventArgs e)
        {
            (DataContext as ViewModels.NewUserDialogViewModel).Password = PasswordBox1.Password;
        }

        private void PasswordBox1_PasswordChanged(object sender, RoutedEventArgs e)
        {
            (DataContext as ViewModels.NewUserDialogViewModel).PasswordConfirmation = PasswordBox2.Password;
        }
    }
}
