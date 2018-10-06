using Controls.Views;
using LabDbContext;
using Microsoft.Practices.Prism.Mvvm;
using Prism.Regions;
using System.Windows.Controls;

namespace Projects.Views
{
    public partial class ProjectInfo : UserControl, INavigationAware, IView
    {
        #region Constructors

        public ProjectInfo(IRegionManager regionManager)
        {
            InitializeComponent();
            regionManager.RegisterViewWithRegion(RegionNames.ProjectBatchListRegion,
                                                typeof(BatchListControl));
            regionManager.RegisterViewWithRegion(RegionNames.ProjectExternalReportListRegion,
                                                typeof(Controls.Resources.ExternalReportList));
            regionManager.RegisterViewWithRegion(RegionNames.ProjectReportListRegion,
                                                typeof(ReportListControl));
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
            (DataContext as ViewModels.ProjectInfoViewModel).ProjectInstance
                = ncontext.Parameters["ObjectInstance"] as Project;
        }

        #endregion Methods
    }
}