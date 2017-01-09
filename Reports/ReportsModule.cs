using Microsoft.Practices.Unity;
using Prism.Events;
using Prism.Modularity;
using Prism.Regions;
using System;

namespace Reports
{
    public class ReportsModule : IModule
    {
        IEventAggregator _eventAggregator;
        IRegionManager _regionManager;
        IUnityContainer _container;

        public ReportsModule(IRegionManager regionManager, IEventAggregator eventAggregator,
            IUnityContainer container)
        {
            _container = container;
            _eventAggregator = eventAggregator;
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            _container.RegisterType<Object, Views.ReportsView>(ReportsViewNames.ReportsView);
            _regionManager.RegisterViewWithRegion("MainNavigationRegion", typeof(Views.ReportsNavigationItem));
        }
    }
}