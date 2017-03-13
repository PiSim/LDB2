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
            _container.RegisterType<Object, Views.ReportMainView>(ViewNames.ReportMainView);
            _container.RegisterType<Object, Views.ReportEditView>(ViewNames.ReportEditView);
            _container.RegisterType<Object, Views.ExternalReportMainView>(ViewNames.ExternalReportMainView);
            _container.RegisterType<Object, Views.ExternalReportEditView>(ViewNames.ExternalReportEditView);

            _regionManager.RegisterViewWithRegion(Navigation.RegionNames.BatchReportListRegion,
                                                typeof(Views.ReportList));
            _regionManager.RegisterViewWithRegion(Navigation.RegionNames.CurrentUserMainReportListRegion,
                                                typeof(Views.ReportList));
            _regionManager.RegisterViewWithRegion(Navigation.RegionNames.MethodReportListRegion,
                                                typeof(Views.ReportList));
            _regionManager.RegisterViewWithRegion(Navigation.RegionNames.ProjectReportListRegion,
                                                typeof(Views.ReportList));
            _regionManager.RegisterViewWithRegion(Navigation.RegionNames.SpecificationReportListRegion,
                                                typeof(Views.ReportList));

            _regionManager.RegisterViewWithRegion(Navigation.RegionNames.MainNavigationRegion
                                                , typeof(Views.ExternalReportsNavigationItem));
            _regionManager.RegisterViewWithRegion(Navigation.RegionNames.MainNavigationRegion
                                                , typeof(Views.ReportsNavigationItem));
        }
    }
}