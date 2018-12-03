using Controls.Views;
using Prism.Mvvm;
using Prism.Regions;
using System.Windows.Controls;

namespace Materials.Views
{
    /// <summary>
    /// Interaction logic for BatchesView.xaml
    /// </summary>
    public partial class BatchMain : UserControl
    {
        #region Constructors

        public BatchMain(IRegionManager regionManager)
        {
            InitializeComponent();
            regionManager.RegisterViewWithRegion(RegionNames.BatchStatusListRegion,
                                                typeof(Views.BatchStatusList));
            regionManager.RegisterViewWithRegion(RegionNames.SampleArchiveRegion,
                                                typeof(Views.SampleArchive));
            regionManager.RegisterViewWithRegion(RegionNames.SampleLongTermStorageRegion,
                                                typeof(Views.SampleLongTermStorage));
        }

        #endregion Constructors
    }
}