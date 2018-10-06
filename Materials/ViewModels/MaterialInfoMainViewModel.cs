using Controls.Views;

namespace Materials.ViewModels
{
    public class MaterialInfoMainViewModel
    {
        #region Constructors

        public MaterialInfoMainViewModel()
        {
        }

        #endregion Constructors

        #region Properties

        public string MaterialInfoAspectRegionName => RegionNames.MaterialInfoAspectRegion;

        public string MaterialInfoColourRegionName => RegionNames.MaterialInfoColourRegion;

        public string MaterialInfoExternalConstructionRegionName => RegionNames.MaterialInfoExternalCostructionRegion;
        public string MaterialInfoMaterialRegionName => RegionNames.MaterialInfoMaterialRegion;

        #endregion Properties
    }
}