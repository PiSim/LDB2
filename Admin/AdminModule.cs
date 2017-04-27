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
            _container.RegisterType<IAdminServiceProvider, AdminServiceProvider>(new ContainerControlledLifetimeManager());
            
            _container.RegisterType<Object, Views.AdminMain>
                (AdminViewNames.AdminMainView);

            _container.RegisterType<ViewModels.AdminMainViewModel>();
            _container.RegisterType<Views.NewUserDialog>();

            _regionManager.RegisterViewWithRegion(RegionNames.MainNavigationRegion, 
                                                    typeof(Views.AdminNavigationItem));
        }
    }
}