using Prism.Modularity;
using Prism.Regions;
using System;

namespace Materials
{
    public class MaterialsModule : IModule
    {
        IRegionManager _regionManager;

        public MaterialsModule(RegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            throw new NotImplementedException();
        }
    }
}