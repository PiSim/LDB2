using Controls.Views;
using Microsoft.Practices.Prism.Mvvm;
using Prism.Regions;
using System.Windows.Controls;

namespace Materials.Views
{
    /// <summary>
    /// Logica di interazione per MaterialInfoMain.xaml
    /// </summary>
    public partial class MaterialInfoMain : UserControl, IView
    {
        #region Constructors

        public MaterialInfoMain(IRegionManager regionManager)
        {
            InitializeComponent();
            regionManager.RegisterViewWithRegion(RegionNames.MaterialInfoAspectRegion,
                                                typeof(Views.AspectMain));
            regionManager.RegisterViewWithRegion(RegionNames.MaterialInfoColourRegion,
                                                typeof(Views.ColourMain));
            regionManager.RegisterViewWithRegion(RegionNames.MaterialInfoMaterialRegion,
                                                typeof(Views.MaterialMain));
            regionManager.RegisterViewWithRegion(RegionNames.MaterialInfoExternalCostructionRegion,
                                                typeof(Views.ExternalConstructionMain));
        }

        #endregion Constructors
    }
}