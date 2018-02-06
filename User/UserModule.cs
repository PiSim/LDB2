using Infrastructure;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using System;
using Unity;
using Unity.Lifetime;

namespace User
{
    [Module(ModuleName = "UserModule")]
    public class UserModule : IModule
    {
        private IRegionManager _regionManager;
        private IUnityContainer _container;

        public UserModule(IRegionManager regionManager,
                        IUnityContainer container)
        {
            _container = container;
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            _container.RegisterType<Object, Views.CurrentUserMain>(UserViewNames.CurrentUserMain);

            _container.RegisterType<ViewModels.CurrentUserMainViewModel>();

            _container.RegisterType<UserServiceProvider>(new ContainerControlledLifetimeManager());
            _container.Resolve<UserServiceProvider>();
        }
    }
}