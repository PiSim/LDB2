using Controls.Views;
using LabDbContext;
using Prism.Mvvm;
using Prism.Regions;
using System.Windows.Controls;

namespace Specifications.Views
{
    /// <summary>
    /// Interaction logic for MethodEditView.xaml
    /// </summary>
    public partial class MethodEdit : UserControl, INavigationAware
    {
        #region Constructors

        public MethodEdit(IRegionManager regionManager)
        {
            InitializeComponent();
            regionManager.RegisterViewWithRegion(RegionNames.MethodEditSpecificationListRegion,
                                                typeof(Controls.Views.SpecificationList));
            regionManager.RegisterViewWithRegion(RegionNames.MethodFileRegion,
                                                typeof(FileListControl));
            regionManager.RegisterViewWithRegion(RegionNames.MethodReportListRegion,
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
            (DataContext as ViewModels.MethodEditViewModel).MethodInstance =
               ncontext.Parameters["ObjectInstance"] as Method;
        }

        #endregion Methods
    }
}