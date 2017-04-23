using Infrastructure;
using System.Windows.Controls;

namespace Reports.Views
{
    /// <summary>
    /// Interaction logic for ReportsNavigationItem.xaml
    /// </summary>
    public partial class ReportsNavigationItem : UserControl, IModuleNavigationTag
    {
        public ReportsNavigationItem()
        {
            InitializeComponent();
        }

        public string ViewName
        {
            get { return ViewNames.ReportMain; }
        }
    }
}
