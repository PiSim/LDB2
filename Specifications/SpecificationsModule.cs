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

            _container.RegisterType<ISpecificationService, SpecificationService>(new ContainerControlledLifetimeManager());
            _container.Resolve<SpecificationService>();

            _container.RegisterType<Object, Views.AddMethod>(SpecificationViewNames.AddMethod);
            _container.RegisterType<Object, Views.ControlPlanEdit>(SpecificationViewNames.ControlPlanEdit);
            _container.RegisterType<Object, Views.MethodEdit>(SpecificationViewNames.MethodEdit);
            _container.RegisterType<Object, Views.SpecificationEdit>(SpecificationViewNames.SpecificationEdit);
            _container.RegisterType<Object, Views.SpecificationVersionEdit>(SpecificationViewNames.SpecificationVersionEdit);
            _container.RegisterType<Object, Views.SpecificationVersionList>(SpecificationViewNames.SpecificationVersionList);
            _container.RegisterType<Object, Views.StandardEdit>(SpecificationViewNames.StandardEdit);
            _container.RegisterType<Object, Views.StandardMain>(SpecificationViewNames.StandardMain);

            _container.RegisterType<Views.MethodCreationDialog>();
            _container.RegisterType<Views.SpecificationCreationDialog>();

            _container.RegisterType<ViewModels.MethodCreationDialogViewModel>();
            _container.RegisterType<ViewModels.MethodEditViewModel>();
            _container.RegisterType<ViewModels.MethodMainViewModel>();
            _container.RegisterType<ViewModels.SpecificationCreationDialogViewModel>();
            _container.RegisterType<ViewModels.SpecificationMainViewModel>();
            _container.RegisterType<ViewModels.SpecificationEditViewModel>();
            _container.RegisterType<ViewModels.SpecificationVersionEditViewModel>();
           

            _regionManager.RegisterViewWithRegion(RegionNames.MethodEditSpecificationListRegion,
                                                typeof(Controls.Views.SpecificationList));
            _regionManager.RegisterViewWithRegion(RegionNames.MethodMainRegion,
                                                typeof(Views.MethodMain));
            _regionManager.RegisterViewWithRegion(RegionNames.SpecificationMainRegion,
                                                typeof(Views.SpecificationMain));
            _regionManager.RegisterViewWithRegion(RegionNames.SpecificationMainListRegion,
                                                typeof(Controls.Views.SpecificationList));
            _regionManager.RegisterViewWithRegion(RegionNames.StandardMainRegion,
                                                typeof(Views.StandardList));

            // ADD Specification tag to navigation Menu if user is authorized

            if (principal.IsInRole(UserRoleNames.SpecificationView))
                _regionManager.RegisterViewWithRegion(RegionNames.MainNavigationRegion, 
                                                    typeof(Views.SpecificationNavigationItem));
        }
    }
}