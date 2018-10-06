using Controls.Views;
using Infrastructure;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Security.Principal;
using System.Threading;

namespace Reports
{
    [Module(ModuleName = "ReportsModule")]
    public class ReportsModule : IModule
    {
        #region Constructors

        public ReportsModule()
        {
        }

        #endregion Constructors

        #region Methods

        public void OnInitialized(IContainerProvider containerProvider)
        {
            IPrincipal principal = Thread.CurrentPrincipal;
            IRegionManager regionManager = containerProvider.Resolve<IRegionManager>();

            if (principal.IsInRole(UserRoleNames.TestSearchView))
                regionManager.RegisterViewWithRegion(RegionNames.MainNavigationRegion,
                                                    typeof(Views.TestSearchNavigationItem));

            if (principal.IsInRole(UserRoleNames.ExternalReportView))
                regionManager.RegisterViewWithRegion(RegionNames.MainNavigationRegion
                                                    , typeof(Views.ExternalReportsNavigationItem));
            if (principal.IsInRole(UserRoleNames.ReportView))
                regionManager.RegisterViewWithRegion(RegionNames.MainNavigationRegion
                                                    , typeof(Views.ReportsNavigationItem));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IReportService, ReportService>();

            containerRegistry.Register<Object, Views.ReportMain>(ViewNames.ReportMain);
            containerRegistry.Register<Object, Views.ReportEdit>(ViewNames.ReportEditView);
            containerRegistry.Register<Object, Views.ExternalReportMain>(ViewNames.ExternalReportMainView);
            containerRegistry.Register<Object, Views.ExternalReportEdit>(ViewNames.ExternalReportEditView);
            containerRegistry.Register<Object, Views.TestSearchMainView>(ViewNames.TestSearchMain);
        }

        #endregion Methods
    }
}