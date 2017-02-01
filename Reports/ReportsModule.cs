using Microsoft.Practices.Unity;
using Prism.Events;
using Prism.Modularity;
using Prism.Regions;
using System;

namespace Reports
{
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

            _regionManager.RegisterViewWithRegion(Navigation.RegionNames.MainNavigationRegion
                                                , typeof(Views.ReportsNavigationItem));
        }
    }
}