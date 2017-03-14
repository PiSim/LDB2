using Infrastructure;
using System.Windows.Controls;

namespace Admin.Views
{
    /// <summary>
    /// Interaction logic for AdminNavigationItem.xaml
    /// </summary>
    public partial class AdminNavigationItem : UserControl, Navigation.IModuleNavigationTag
    {
        public AdminNavigationItem()
        {
            InitializeComponent();
        }

        public string ViewName
        {
            get { return AdminViewNames.AdminMainView; }
        }
    }
}
