using Controls.Views;
using Infrastructure;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System;

namespace Admin
{
    public class AdminModule : IModule
    {
        #region Constructors

        public AdminModule()
        {
        }

        #endregion Constructors

        #region Methods

        public void Initialize()
        {
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            IRegionManager regionManager = containerProvider.Resolve<IRegionManager>();

            regionManager.RegisterViewWithRegion(RegionNames.MainNavigationRegion,
                                                    typeof(Views.AdminNavigationItem));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IAdminService, AdminService>();

            containerRegistry.Register<Object, Views.AdminMain>(AdminViewNames.AdminMainView);
            containerRegistry.Register<Object, Views.InstrumentTypeEdit>(AdminViewNames.InstrumentTypeEditView);
            containerRegistry.Register<Object, Views.OrganizationEdit>(OrganizationViewNames.OrganizationEditView);
            containerRegistry.Register<Object, Views.OrganizationsMain>(OrganizationViewNames.OrganizationMainView);
            containerRegistry.Register<Object, Views.UserEdit>(AdminViewNames.UserEditView);

            containerRegistry.Register<Views.NewUserDialog>();
        }

        #endregion Methods
    }
}