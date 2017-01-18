using Prism.Modularity;
using Prism.Regions;
using System;

namespace LabDB
{
    public class LabDBModule : IModule
    {
        IRegionManager _regionManager;

        public LabDBModule(RegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            throw new NotImplementedException();
        }
    }
}