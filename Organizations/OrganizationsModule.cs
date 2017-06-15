using Infrastructure;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using System;

namespace Organizations
{
    public class OrganizationsModule : IModule
    {
        IRegionManager _regionManager;
        IUnityContainer _container;

        public OrganizationsModule(RegionManager regionManager,
                                    IUnityContainer container)
        {
            _container = container;
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            DBPrincipal principal = _container.Resolve<DBPrincipal>();
            if (principal.IsInRole(UserRoleNames.Admin))
                _regionManager.RegisterViewWithRegion(RegionNames.MainNavigationRegion,
                                                typeof(Views.OrganizationsNavigationItem));

            _container.RegisterType<ViewModels.OrganizationEditViewModel>();
            _container.RegisterType<ViewModels.OrganizationsMainViewModel>();

            _container.RegisterType<Object, Views.OrganizationEdit>(OrganizationViewNames.OrganizationEditView);
            _container.RegisterType<Object, Views.OrganizationsMain>(OrganizationViewNames.OrganizationMainView);
            

            _container.RegisterType<OrganizationServiceProvider>(
                new ContainerControlledLifetimeManager());
            _container.Resolve<OrganizationServiceProvider>();
        }
    }
}