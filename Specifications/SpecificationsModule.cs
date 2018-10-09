using Controls.Views;
using Infrastructure;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System;

using System.Threading;

namespace Specifications
{
    public class SpecificationsModule : IModule
    {
        #region Constructors

        public SpecificationsModule()
        {
        }

        #endregion Constructors

        #region Methods

        public void OnInitialized(IContainerProvider containerProvider)
        {
            // ADD Specification tag to navigation Menu if user is authorized

            IRegionManager regionManager = containerProvider.Resolve<IRegionManager>();

            if (Thread.CurrentPrincipal.IsInRole(UserRoleNames.SpecificationView))
                regionManager.RegisterViewWithRegion(RegionNames.MainNavigationRegion,
                                                    typeof(Views.SpecificationNavigationItem));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<ISpecificationService, SpecificationService>();

            containerRegistry.Register<Object, Views.AddMethod>(SpecificationViewNames.AddMethod);
            containerRegistry.Register<Object, Views.ControlPlanEdit>(SpecificationViewNames.ControlPlanEdit);
            containerRegistry.Register<Object, Views.MethodEdit>(SpecificationViewNames.MethodEdit);
            containerRegistry.Register<Object, Views.SpecificationEdit>(SpecificationViewNames.SpecificationEdit);
            containerRegistry.Register<Object, Views.SpecificationVersionEdit>(SpecificationViewNames.SpecificationVersionEdit);
            containerRegistry.Register<Object, Views.SpecificationVersionList>(SpecificationViewNames.SpecificationVersionList);
            containerRegistry.RegisterForNavigation<Object, Views.StandardMain>(SpecificationViewNames.StandardMain);

            containerRegistry.Register<Views.MethodCreationDialog>();
            containerRegistry.Register<Views.SpecificationCreationDialog>();

            containerRegistry.Register<ViewModels.MethodCreationDialogViewModel>();
            containerRegistry.Register<ViewModels.MethodEditViewModel>();
            containerRegistry.Register<ViewModels.MethodMainViewModel>();
            containerRegistry.Register<ViewModels.SpecificationCreationDialogViewModel>();
            containerRegistry.Register<ViewModels.SpecificationMainViewModel>();
            containerRegistry.Register<ViewModels.SpecificationEditViewModel>();
            containerRegistry.Register<ViewModels.SpecificationVersionEditViewModel>();
        }

        #endregion Methods
    }
}