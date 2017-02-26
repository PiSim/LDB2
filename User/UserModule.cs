using Prism.Modularity;
using Prism.Regions;
using System;

namespace User
{
    public class UserModule : IModule
    {
        IRegionManager _regionManager;

        public UserModule(RegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            throw new NotImplementedException();
        }
    }
}