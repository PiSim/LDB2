using Infrastructure;
using System.Windows.Controls;

namespace Reports.Views
{
    /// <summary>
    /// Interaction logic for ReportsNavigationItem.xaml
    /// </summary>
    public partial class ReportsNavigationItem : UserControl, IModuleNavigationTag
    {
        #region Constructors

        public ReportsNavigationItem()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        public string ViewName => ViewNames.ReportMain;

        #endregion Properties
    }
}