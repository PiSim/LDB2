using Infrastructure;
using System.Windows.Controls;

namespace Reports.Views
{
    /// <summary>
    /// Interaction logic for ReportsNavigationItem.xaml
    /// </summary>
    public partial class TestSearchNavigationItem : UserControl, IModuleNavigationTag
    {
        #region Constructors

        public TestSearchNavigationItem()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        public string ViewName => ViewNames.TestSearchMain;

        #endregion Properties
    }
}