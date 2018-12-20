using LInst;
using System.Windows;

namespace Admin.Views
{
    /// <summary>
    /// Interaction logic for NewUserDialog.xaml
    /// </summary>
    public partial class NewUserDialog : Window
    {
        #region Constructors

        public NewUserDialog()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        public User NewUserInstance => (DataContext as ViewModels.NewUserDialogViewModel).UserInstance;

        #endregion Properties
    }
}