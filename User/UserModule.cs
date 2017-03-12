using Infrastructure;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using System;

namespace User
{
    public class UserModule : IModule
    {
        private IRegionManager _regionManager;
        private UnityContainer _container;

        public UserModule(RegionManager regionManager,
                        UnityContainer container)
        {
            _container = container;
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            _container.RegisterType<Object, Views.CurrentUserMain>(UserViewNames.CurrentUserMain);

            _container.RegisterType<ViewModels.CurrentUserMainViewModel>();
        }
    }
}