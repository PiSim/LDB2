using Infrastructure;
using System.Windows.Controls;

namespace Reports.Views
{
    /// <summary>
    /// Interaction logic for ReportsNavigationItem.xaml
    /// </summary>
    public partial class TestSearchNavigationItem : UserControl, IModuleNavigationTag
    {
        public TestSearchNavigationItem()
        {
            InitializeComponent();
        }

        public string ViewName
        {
            get { return ViewNames.TestSearchMain; }
        }
    }
}
