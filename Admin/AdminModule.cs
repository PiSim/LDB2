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
            _container.RegisterType<IAdminService, AdminService>(new ContainerControlledLifetimeManager());
            _container.Resolve<IAdminService>();

            _container.RegisterType<Object, Views.AdminMain>(AdminViewNames.AdminMainView);
            _container.RegisterType<Object, Views.InstrumentTypeEdit>(AdminViewNames.InstrumentTypeEditView);
            _container.RegisterType<Object, Views.MeasurableQuantityEdit>(AdminViewNames.MeasurableQuantityEdit);
            _container.RegisterType<Object, Views.OrganizationEdit>(OrganizationViewNames.OrganizationEditView);
            _container.RegisterType<Object, Views.OrganizationsMain>(OrganizationViewNames.OrganizationMainView);
            _container.RegisterType<Object, Views.UserEdit>(AdminViewNames.UserEditView);

            _container.RegisterType<ViewModels.AdminMainViewModel>();
            _container.RegisterType<ViewModels.MeasurableQuantityMainViewModel>();
            _container.RegisterType<ViewModels.NewUserDialogViewModel>();
            _container.RegisterType<ViewModels.OrganizationEditViewModel>();
            _container.RegisterType<ViewModels.OrganizationsMainViewModel>();
            _container.RegisterType<ViewModels.UserEditViewModel>();
            _container.RegisterType<ViewModels.UserMainViewModel>();

            _container.RegisterType<Views.NewUserDialog>();



            _regionManager.RegisterViewWithRegion(RegionNames.MainNavigationRegion, 
                                                    typeof(Views.AdminNavigationItem));
            _regionManager.RegisterViewWithRegion(RegionNames.AdminUserMainRegion,
                                                    typeof(Views.UserMain));
            _regionManager.RegisterViewWithRegion(RegionNames.InstrumentTypeManagementRegion,
                                                    typeof(Views.InstrumentTypeMain));
            _regionManager.RegisterViewWithRegion(RegionNames.InstrumentUtilizationAreasRegion,
                                                    typeof(Views.InstrumentUtilizationAreaMain));
            _regionManager.RegisterViewWithRegion(RegionNames.MeasurableQuantityManagementRegion,
                                                    typeof(Views.MeasurableQuantityMain));
            _regionManager.RegisterViewWithRegion(RegionNames.UnitOfMeasurementManagementRegion,
                                                    typeof(Views.MeasurementUnitMain));
            _regionManager.RegisterViewWithRegion(RegionNames.OrganizationRoleManagementRegion,
                                                typeof(Views.OrganizationsMain));
            _regionManager.RegisterViewWithRegion(RegionNames.PeopleManagementRegion,
                                                    typeof(Views.PeopleMain));
            _regionManager.RegisterViewWithRegion(RegionNames.PropertyManagementRegion,
                                                    typeof(Views.PropertyMain));
        }
    }
}