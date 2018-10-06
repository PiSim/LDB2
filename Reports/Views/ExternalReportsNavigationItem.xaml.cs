using Infrastructure;
using System.Windows.Controls;

namespace Reports.Views
{
    /// <summary>
    /// Interaction logic for ExternalReportsNavigationItem.xaml
    /// </summary>
    public partial class ExternalReportsNavigationItem : UserControl, IModuleNavigationTag
    {
        #region Constructors

        public ExternalReportsNavigationItem()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        public string ViewName => ViewNames.ExternalReportMainView;

        #endregion Properties
    }
}