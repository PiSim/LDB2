using Prism.Modularity;
using Prism.Regions;
using System;

namespace Jobs
{
    public class JobsModule : IModule
    {
        IRegionManager _regionManager;

        public JobsModule(RegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            throw new NotImplementedException();
        }
    }
}