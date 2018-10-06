using Infrastructure;
using System.Windows.Controls;

namespace Admin.Views
{
    /// <summary>
    /// Interaction logic for AdminNavigationItem.xaml
    /// </summary>
    public partial class AdminNavigationItem : UserControl, IModuleNavigationTag
    {
        #region Constructors

        public AdminNavigationItem()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        public string ViewName => AdminViewNames.AdminMainView;

        #endregion Properties
    }
}