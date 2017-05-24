using Prism.Modularity;
using Prism.Regions;
using System;

namespace Services
{
    public class ServicesModule : IModule
    {
        IRegionManager _regionManager;

        public ServicesModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            throw new NotImplementedException();
        }
    }
}