using Microsoft.Practices.Unity;
using Navigation;
using Prism.Modularity;
using Prism.Regions;
using System;

namespace Specifications
{
    public class SpecificationsModule : IModule
    {
        IRegionManager _regionManager;
        IUnityContainer _container;

        public SpecificationsModule(IRegionManager regionManager, IUnityContainer container)
        {
            _container = container;
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            _container.RegisterType<Object, Views.MethodMain>(ViewNames.MethodMainView);
            _container.RegisterType<Object, Views.SpecificationEditView>(ViewNames.SpecificationsEditView);
            _container.RegisterType<Object, Views.SpecificationMain>(ViewNames.SpecificationsMainView);

            _container.RegisterType<Views.MethodCreationDialog>();
            _container.RegisterType<Views.SpecificationCreationDialog>();

            _container.RegisterType<ViewModels.MethodEditViewModel>();
            _container.RegisterType<ViewModels.MethodMainViewModel>();
            _container.RegisterType<ViewModels.SpecificationMainViewModel>();

            _regionManager.RegisterViewWithRegion("MethodEditRegion",
                                                typeof(Views.MethodEdit));

            _regionManager.RegisterViewWithRegion(RegionNames.MainNavigationRegion, 
                                                typeof(Views.MethodNavigationItem));
            _regionManager.RegisterViewWithRegion(RegionNames.MainNavigationRegion, 
                                                typeof(Views.SpecificationNavigationItem));
        }
    }
}