using Controls.Views;
using LabDbContext;
using Prism.Regions;
using System.Windows.Controls;

namespace Specifications.Views
{
    /// <summary>
    /// Interaction logic for SpecificationEditView.xaml
    /// </summary>
    public partial class SpecificationEdit : UserControl, INavigationAware
    {
        #region Constructors

        public SpecificationEdit(IRegionManager regionManager)
        {
            InitializeComponent();

            regionManager.RegisterViewWithRegion(RegionNames.SpecificationEditFileRegion,
                                                typeof(FileListControl));
            regionManager.RegisterViewWithRegion(RegionNames.SpecificationReportListRegion,
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
            (DataContext as ViewModels.SpecificationEditViewModel).SpecificationInstance
                = ncontext.Parameters["ObjectInstance"] as Specification;
        }

        #endregion Methods
    }
}