using Controls.Views;
using LabDbContext;
using Prism.Regions;
using System.Windows.Controls;

namespace Materials.Views
{
    /// <summary>
    /// Interaction logic for BatchInfoView.xaml
    /// </summary>
    public partial class BatchInfo : UserControl, INavigationAware
    {
        #region Constructors

        public BatchInfo(IRegionManager regionManager)
        {
            InitializeComponent();
            regionManager.RegisterViewWithRegion(RegionNames.BatchReportListRegion,
                                                typeof(ReportListControl));
            regionManager.RegisterViewWithRegion(RegionNames.BatchExternalReportListRegion,
                                                typeof(Controls.Resources.ExternalReportList));
        }

        #endregion Constructors

        #region Methods

        public bool IsNavigationTarget(NavigationContext ncontext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext ncontext)
        {
        }

        public void OnNavigatedTo(NavigationContext ncontext)
        {
            (DataContext as ViewModels.BatchInfoViewModel).BatchInstance
                = ncontext.Parameters["ObjectInstance"] as Batch;
        }

        #endregion Methods
    }
}