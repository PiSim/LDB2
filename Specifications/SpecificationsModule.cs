using Microsoft.Practices.Unity;
using Infrastructure;
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
            _container.RegisterType<SpecificationServiceProvider>(new ContainerControlledLifetimeManager());
            _container.Resolve<SpecificationServiceProvider>();

            _container.RegisterType<Object, Views.MethodEdit>(ViewNames.MethodEditView);
            _container.RegisterType<Object, Views.MethodMain>(ViewNames.MethodMainView);
            _container.RegisterType<Object, Views.SpecificationEdit>(ViewNames.SpecificationsEditView);
            _container.RegisterType<Object, Views.SpecificationMain>(ViewNames.SpecificationsMainView);

            _container.RegisterType<Views.MethodCreationDialog>();
            _container.RegisterType<Views.SpecificationCreationDialog>();

            _container.RegisterType<ViewModels.MethodCreationDialogViewModel>();
            _container.RegisterType<ViewModels.MethodEditViewModel>();
            _container.RegisterType<ViewModels.MethodMainViewModel>();
            _container.RegisterType<ViewModels.SpecificationCreationDialogViewModel>();
            _container.RegisterType<ViewModels.SpecificationMainViewModel>();
            _container.RegisterType<ViewModels.SpecificationEditViewModel>();

            _regionManager.RegisterViewWithRegion(RegionNames.MainNavigationRegion, 
                                                typeof(Views.MethodNavigationItem));
            _regionManager.RegisterViewWithRegion(RegionNames.MainNavigationRegion, 
                                                typeof(Views.SpecificationNavigationItem));
            _regionManager.RegisterViewWithRegion(RegionNames.MethodIssueRegion,
                                                typeof(Views.StandardIssueControl));
            _regionManager.RegisterViewWithRegion(RegionNames.SpecificationIssueRegion,
                                                typeof(Views.StandardIssueControl));
        }
    }
}