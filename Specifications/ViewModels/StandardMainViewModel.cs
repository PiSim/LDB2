using Controls.Views;
using Microsoft.Practices.Prism.Mvvm;

namespace Specifications.ViewModels
{
    public class StandardMainViewModel : BindableBase
    {
        #region Constructors

        public StandardMainViewModel()
        {
        }

        #endregion Constructors

        #region Properties

        public string MethodMainRegionName => RegionNames.MethodMainRegion;
        public string SpecificationMainRegionName => RegionNames.SpecificationMainRegion;
        public string StandardMainRegionName => RegionNames.StandardMainRegion;

        #endregion Properties
    }
}