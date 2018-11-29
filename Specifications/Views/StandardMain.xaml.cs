using Controls.Views;
using Prism.Regions;
using System.Windows.Controls;

namespace Specifications.Views
{
    /// <summary>
    /// Logica di interazione per StandardMain.xaml
    /// </summary>
    public partial class StandardMain : UserControl
    {
        #region Constructors

        public StandardMain(IRegionManager regionManager)
        {
            regionManager.RegisterViewWithRegion(RegionNames.MethodMainRegion,
                                                typeof(Views.MethodMain));
            regionManager.RegisterViewWithRegion(RegionNames.SpecificationMainRegion,
                                                typeof(Views.SpecificationMain));
            regionManager.RegisterViewWithRegion(RegionNames.SpecificationMainListRegion,
                                                typeof(Controls.Views.SpecificationList));
            InitializeComponent();
        }

        #endregion Constructors
    }
}