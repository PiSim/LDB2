using Controls.Views;
using Prism.Mvvm;
using Prism.Regions;
using System.Windows.Controls;

namespace Reports.Views
{
    /// <summary>
    /// Interaction logic for ExternalReportMainView.xaml
    /// </summary>
    public partial class ExternalReportMain : UserControl
    {
        #region Constructors

        public ExternalReportMain(IRegionManager regionManager)
        {
            InitializeComponent();
            regionManager.RegisterViewWithRegion(RegionNames.MainExternalReportListRegion,
                                                typeof(Controls.Resources.ExternalReportList));
        }

        #endregion Constructors
    }
}