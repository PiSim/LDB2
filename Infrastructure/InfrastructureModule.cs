using Prism.Events;
using Prism.Modularity;
using Prism.Regions;
using System;

namespace Infrastructure
{
    [Module(ModuleName = "InfrastructureModule")]
    public class InfrastructureModule : IModule
    {
        IRegionManager _regionManager;

        public InfrastructureModule(RegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            
        }
    }
}