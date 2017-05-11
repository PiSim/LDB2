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
    public partial class NewUserDialog : Window, IView
    {
        public NewUserDialog()
        {
            InitializeComponent();
        }

        public User NewUserInstance
        {
            get { return (DataContext as ViewModels.NewUserDialogViewModel).UserInstance; }
        }
    }
}
