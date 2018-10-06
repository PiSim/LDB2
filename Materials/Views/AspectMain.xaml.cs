using Controls.Views;
using Prism.Regions;
using System.Windows.Controls;

namespace Materials.Views
{
    /// <summary>
    /// Logica di interazione per AspectMain.xaml
    /// </summary>
    public partial class AspectMain : UserControl
    {
        #region Constructors

        public AspectMain(IRegionManager regionManager)
        {
            InitializeComponent();
            regionManager.RegisterViewWithRegion(RegionNames.AspectDetailBatchListRegion,
                                                typeof(BatchListControl));
        }

        #endregion Constructors
    }
}