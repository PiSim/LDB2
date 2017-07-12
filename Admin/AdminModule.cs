using Infrastructure;
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
            _container.RegisterType<AdminServiceProvider>(new ContainerControlledLifetimeManager());
            _container.Resolve<AdminServiceProvider>();

            _container.RegisterType<Object, Views.AdminMain>(AdminViewNames.AdminMainView);
            _container.RegisterType<Object, Views.UserEdit>(AdminViewNames.UserEditView);

            _container.RegisterType<ViewModels.AdminMainViewModel>();
            _container.RegisterType<ViewModels.MeasurableQuantityMainViewModel>();
            _container.RegisterType<ViewModels.NewUserDialogViewModel>();
            _container.RegisterType<ViewModels.UserEditViewModel>();
            _container.RegisterType<ViewModels.UserMainViewModel>();

            _container.RegisterType<Views.NewUserDialog>();

            _regionManager.RegisterViewWithRegion(RegionNames.MainNavigationRegion, 
                                                    typeof(Views.AdminNavigationItem));
            _regionManager.RegisterViewWithRegion(RegionNames.AdminUserMainRegion,
                                                    typeof(Views.UserMain));
            _regionManager.RegisterViewWithRegion(RegionNames.MeasurableQuantityManagementRegion,
                                                    typeof(Views.MeasurableQuantityMain));
        }
    }
}