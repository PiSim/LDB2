using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using System;

namespace Masters
{
    public class MastersModule : IModule
    {
        IRegionManager _regionManager;
        IUnityContainer _container;

        public MastersModule(IRegionManager regionManager, IUnityContainer container)
        {
            _container = container;
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            _regionManager.RegisterViewWithRegion
                (Navigation.RegionNames.MainNavigationRegion, typeof(Views.MasterNavigationItem));
        }
    }
}