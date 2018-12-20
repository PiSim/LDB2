using Controls.Views;
using Prism.Mvvm;

namespace Specifications.ViewModels
{
    public class StandardMainViewModel : BindableBase
    {
        #region Properties

        public string MethodMainRegionName => RegionNames.MethodMainRegion;
        public string SpecificationMainRegionName => RegionNames.SpecificationMainRegion;

        #endregion Properties
    }
}