using Infrastructure;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using System;

namespace People
{
    [Module(ModuleName = "PeopleModule")]
    public class PeopleModule : IModule
    {
        IRegionManager _regionManager;
        IUnityContainer _container;

        public PeopleModule(IRegionManager regionManager,
                            IUnityContainer container)
        {
            _regionManager = regionManager;
            _container = container;
        }

        public void Initialize()
        {
            DBPrincipal principal = _container.Resolve<DBPrincipal>();

            if (principal.IsInRole(UserRoleNames.Admin))
                _regionManager.RegisterViewWithRegion(RegionNames.MainNavigationRegion,
                                                    typeof(Views.PeopleNavigationItem));

            _container.RegisterType<Object, Views.PeopleMain>(PeopleViewNames.PeopleMainView);

            _container.RegisterType<ViewModels.PeopleMainViewModel>();
        }
    }
}