using Controls.Views;
using LabDbContext;
using Prism.Mvvm;
using Prism.Regions;
using System.Windows.Controls;

namespace Materials.Views
{
    /// <summary>
    /// Logica di interazione per ExternalConstructionDetail.xaml
    /// </summary>
    public partial class ExternalConstructionDetail : UserControl, INavigationAware
    {
        #region Constructors

        public ExternalConstructionDetail(IRegionManager regionManager)
        {
            InitializeComponent();
            regionManager.RegisterViewWithRegion(RegionNames.ExternalConstructionBatchListRegion,
                                                typeof(BatchListControl));
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
            (DataContext as ViewModels.ExternalConstructionDetailViewModel).ExternalConstructionInstance =
               ncontext.Parameters["ObjectInstance"] as ExternalConstruction;
        }

        #endregion Methods
    }
}