using Prism.Modularity;
using Prism.Regions;
using System;

namespace BatchModule
{
    public class BatchModuleModule : IModule
    {
        IRegionManager _regionManager;

        public BatchModuleModule(RegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            throw new NotImplementedException();
        }
    }
}