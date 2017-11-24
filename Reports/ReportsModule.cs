using Infrastructure;
using Microsoft.Practices.Unity;
using Prism.Events;
using Prism.Modularity;
using Prism.Regions;
using System;

namespace Reports
{
    [Module(ModuleName = "ReportsModule")]
    public class ReportsModule : IModule
    {
        DBPrincipal _principal;
        IRegionManager _regionManager;
        IUnityContainer _container;

        public ReportsModule(DBPrincipal principal,
                            IRegionManager regionManager,
                            IUnityContainer container)
        {
            _container = container;
            _principal = principal;
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            _container.RegisterType<IReportService, ReportService>(new ContainerControlledLifetimeManager());
            _container.Resolve<IReportService>();

            _container.RegisterType<Object, Views.ReportMain>(ViewNames.ReportMain);
            _container.RegisterType<Object, Views.ReportEdit>(ViewNames.ReportEditView);
            _container.RegisterType<Object, Views.ExternalReportMain>(ViewNames.ExternalReportMainView);
            _container.RegisterType<Object, Views.ExternalReportEdit>(ViewNames.ExternalReportEditView);

            _container.RegisterType<ViewModels.ReportCreationDialogViewModel>();
            _container.RegisterType<ViewModels.ReportEditViewModel>();
            _container.RegisterType<ViewModels.ReportMainViewModel>();
            _container.RegisterType<ViewModels.ExternalReportEditViewModel>();
            _container.RegisterType<ViewModels.ExternalReportMainViewModel>();

            _regionManager.RegisterViewWithRegion(RegionNames.BatchExternalReportListRegion,
                                                typeof(Views.ExternalReportList));
            _regionManager.RegisterViewWithRegion(RegionNames.MainExternalReportListRegion,
                                                typeof(Views.ExternalReportList));

            _regionManager.RegisterViewWithRegion(RegionNames.BatchReportListRegion,
                                                typeof(Views.ReportList));
            _regionManager.RegisterViewWithRegion(RegionNames.CurrentUserMainReportListRegion,
                                                typeof(Views.ReportList));
            _regionManager.RegisterViewWithRegion(RegionNames.MainReportListRegion,
                                                typeof(Views.ReportList));
            _regionManager.RegisterViewWithRegion(RegionNames.MethodReportListRegion,
                                                typeof(Views.ReportList));
            _regionManager.RegisterViewWithRegion(RegionNames.ProjectReportListRegion,
                                                typeof(Views.ReportList));
            _regionManager.RegisterViewWithRegion(RegionNames.SpecificationReportListRegion,
                                                typeof(Views.ReportList));

            if (_principal.IsInRole(UserRoleNames.ExternalReportView))
                _regionManager.RegisterViewWithRegion(RegionNames.MainNavigationRegion
                                                    , typeof(Views.ExternalReportsNavigationItem));
            if (_principal.IsInRole(UserRoleNames.ReportView))
                _regionManager.RegisterViewWithRegion(RegionNames.MainNavigationRegion
                                                    , typeof(Views.ReportsNavigationItem));
        }
    }
}