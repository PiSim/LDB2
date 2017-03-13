using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using System;

namespace Materials
{
    [Module(ModuleName = "MaterialsModule")]
    public class MaterialsModule : IModule
    {
        private IRegionManager _regionManager;
        private IUnityContainer _container;

        public MaterialsModule(RegionManager regionManager,
                                IUnityContainer container)
        {
            _container = container;
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            _container.RegisterType<Object, Views.BatchInfo>(ViewNames.BatchInfoView);
            _container.RegisterType<Object, Views.BatchMainView>(ViewNames.BatchesView);
            _container.RegisterType<Object, Views.SampleLogView>(ViewNames.SampleLogView);

            _container.RegisterType<ViewModels.BatchInfoViewModel>();

            _regionManager.RegisterViewWithRegion(Navigation.RegionNames.MainNavigationRegion, 
                                                typeof(Views.BatchesNavigationItem));
        }
    }
}