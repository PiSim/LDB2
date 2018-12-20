using LInst;
using Prism.Regions;
using System.Windows.Controls;

namespace Admin.Views
{
    /// <summary>
    /// Interaction logic for UserEdit.xaml
    /// </summary>
    public partial class UserEdit : UserControl, INavigationAware
    {
        #region Constructors

        public UserEdit()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Methods

        public bool IsNavigationTarget(NavigationContext ncontext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext ncontext)
        {
        }

        public void OnNavigatedTo(NavigationContext ncontext)
        {
            (DataContext as ViewModels.UserEditViewModel).UserInstance
                = ncontext.Parameters["ObjectInstance"] as User;
        }

        #endregion Methods
    }
}