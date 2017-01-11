using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using System;

namespace Methods
{
    public class MethodsModule : IModule
    {
        IRegionManager _regionManager;
        IUnityContainer _container;

        public MethodsModule(IRegionManager regionManager, IUnityContainer container)
        {
            _container = container;
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            _container.RegisterType<Object, Views.MethodMainView>
                (MethodsViewNames.MethodMainView);

            _regionManager.RegisterViewWithRegion
                (Navigation.RegionNames.MainNavigationRegion, typeof(Views.MethodNavigationItem));
        }
    }
}