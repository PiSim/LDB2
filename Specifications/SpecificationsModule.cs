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
            DBPrincipal principal = _container.Resolve<DBPrincipal>();

            _container.RegisterType<SpecificationServiceProvider>(new ContainerControlledLifetimeManager());
            _container.Resolve<SpecificationServiceProvider>();

            _container.RegisterType<Object, Views.MethodEdit>(ViewNames.MethodEditView);
            _container.RegisterType<Object, Views.MethodMain>(ViewNames.MethodMainView);
            _container.RegisterType<Object, Views.SpecificationEdit>(ViewNames.SpecificationsEditView);
            _container.RegisterType<Object, Views.SpecificationMain>(ViewNames.SpecificationsMainView);
            _container.RegisterType<Object, Views.SpecificationVersionEdit>(SpecificationViewNames.SpecificationVersionEdit);
            _container.RegisterType<Object, Views.StandardIssueEdit>(SpecificationViewNames.StandardIssueEdit);

            _container.RegisterType<Views.MethodCreationDialog>();
            _container.RegisterType<Views.SpecificationCreationDialog>();

            _container.RegisterType<ViewModels.MethodCreationDialogViewModel>();
            _container.RegisterType<ViewModels.MethodEditViewModel>();
            _container.RegisterType<ViewModels.MethodMainViewModel>();
            _container.RegisterType<ViewModels.SpecificationCreationDialogViewModel>();
            _container.RegisterType<ViewModels.SpecificationMainViewModel>();
            _container.RegisterType<ViewModels.SpecificationEditViewModel>();
            _container.RegisterType<ViewModels.SpecificationVersionEditViewModel>();
            _container.RegisterType<ViewModels.StandardIssueEditViewModel>();

            _regionManager.RegisterViewWithRegion(RegionNames.MainNavigationRegion, 
                                                typeof(Views.MethodNavigationItem));


            // ADD Specification tag to navigation Menu if user is authorized

            if (principal.IsInRole(UserRoleNames.SpecificationView))
                _regionManager.RegisterViewWithRegion(RegionNames.MainNavigationRegion, 
                                                    typeof(Views.SpecificationNavigationItem));


            _regionManager.RegisterViewWithRegion(RegionNames.MethodIssueRegion,
                                                typeof(Views.StandardIssueControl));
            _regionManager.RegisterViewWithRegion(RegionNames.SpecificationIssueRegion,
                                                typeof(Views.StandardIssueControl));
        }
    }
}