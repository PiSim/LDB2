using Controls.Views;
using LabDbContext;
using Microsoft.Practices.Prism.Mvvm;
using Prism.Regions;
using System.Windows.Controls;

namespace Materials.Views
{
    /// <summary>
    /// Logica di interazione per ConstructionDetail.xaml
    /// </summary>
    public partial class MaterialDetail : UserControl, IView, INavigationAware
    {
        #region Constructors

        public MaterialDetail(IRegionManager regionManager)
        {
            InitializeComponent();
            regionManager.RegisterViewWithRegion(RegionNames.MaterialDetailBatchListRegion,
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
            (DataContext as ViewModels.MaterialDetailViewModel).MaterialInstance =
               ncontext.Parameters["ObjectInstance"] as Material;
        }

        #endregion Methods
    }
}