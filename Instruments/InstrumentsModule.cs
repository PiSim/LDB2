using Microsoft.Practices.Unity;
using Navigation;
using Prism.Modularity;
using Prism.Regions;
using System;

namespace Instruments
{
    public class InstrumentsModule : IModule
    {
        IRegionManager _regionManager;
        IUnityContainer _container;

        public InstrumentsModule(RegionManager regionManager, 
                                IUnityContainer container)
        {
            _container = container;
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            _container.RegisterType<Object, Views.InstrumentsMainView>(ViewNames.InstrumentsMainView);

            _regionManager.RegisterViewWithRegion(RegionNames.MainNavigationRegion, 
                                                typeof(Views.InstrumentsNavigationItem));
        }
    }
}