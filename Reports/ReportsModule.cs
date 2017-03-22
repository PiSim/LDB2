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
        IRegionManager _regionManager;
        IUnityContainer _container;

        public ReportsModule(IRegionManager regionManager,
                             IUnityContainer container)
        {
            _container = container;
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            _container.RegisterType<Object, Views.ReportMain>(ViewNames.ReportMain);
            _container.RegisterType<Object, Views.ReportEditView>(ViewNames.ReportEditView);
            _container.RegisterType<Object, Views.ExternalReportMain>(ViewNames.ExternalReportMainView);
            _container.RegisterType<Object, Views.ExternalReportEdit>(ViewNames.ExternalReportEditView);
            
            _container.RegisterType<ViewModels.ReportMainViewModel>();
            _container.RegisterType<ViewModels.ExternalReportEditViewModel>();
            _container.RegisterType<ViewModels.ExternalReportMainViewModel>();

            _container.RegisterType<Views.ReportCreationDialog>();

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

            _regionManager.RegisterViewWithRegion(RegionNames.MainNavigationRegion
                                                , typeof(Views.ExternalReportsNavigationItem));
            _regionManager.RegisterViewWithRegion(RegionNames.MainNavigationRegion
                                                , typeof(Views.ReportsNavigationItem));

            _container.RegisterType<ReportServiceProvider>(new ContainerControlledLifetimeManager());
            _container.Resolve<ReportServiceProvider>();
        }
    }
}