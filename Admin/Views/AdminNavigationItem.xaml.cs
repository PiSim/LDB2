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
using System.Windows.Navigation;
using System.Windows.Shapes;

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
