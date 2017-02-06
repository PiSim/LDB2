using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using System;

namespace Admin
{
    public class AdminModule : IModule
    {
        IRegionManager _regionManager;
        IUnityContainer _container;

        public AdminModule(IRegionManager regionManager, IUnityContainer container)
        {
            _container = container;
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            _container.RegisterType<Object, Views.AdminMainView>
                (AdminViewNames.AdminMainView);

            _regionManager.RegisterViewWithRegion(Navigation.RegionNames.MainNavigationRegion, 
                                                    typeof(Views.AdminNavigationItem));
        }
    }
}