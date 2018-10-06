using Controls.Views;
using Microsoft.Practices.Prism.Mvvm;
using Prism.Regions;
using System.Windows.Controls;

namespace Materials.Views
{
    /// <summary>
    /// Interaction logic for BatchesView.xaml
    /// </summary>
    public partial class BatchMain : UserControl, IView
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