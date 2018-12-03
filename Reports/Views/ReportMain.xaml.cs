using Prism.Mvvm;
using Prism.Regions;
using System.Windows.Controls;

namespace Reports.Views
{
    /// <summary>
    /// Interaction logic for ReportMainView.xaml
    /// </summary>
    public partial class ReportMain : UserControl
    {
        #region Constructors

        public ReportMain(IRegionManager regionManager)
        {
            InitializeComponent();
        }

        #endregion Constructors
    }
}