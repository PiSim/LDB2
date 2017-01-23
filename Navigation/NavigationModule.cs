using Infrastructure;
using Prism.Events;
using Prism.Modularity;
using Prism.Regions;
using System;

namespace Navigation
{
    public class NavigationModule : IModule
    {
        IRegionManager _regionManager;
        IEventAggregator _eventAggregator;

        public NavigationModule(RegionManager regionManager, IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            _eventAggregator.GetEvent<NavigationRequested>().Subscribe(str => OnNavigationRequested(str), true);
        }

        public void OnNavigationRequested(string viewName)
        {
            _regionManager.RequestNavigate("MainRegion", new Uri(viewName, UriKind.Relative));
        }

        public void OnObjectVisualizationRequested(ObjectNavigationToken token)
        {
            NavigationParameters parameters = new NavigationParameters();
            parameters.Add("batch", token.ObjectInstance);
            _regionManager.RequestNavigate(
                RegionNames.MainRegion,
                new Uri(token.ViewName, UriKind.Relative),
                parameters
                );
        }
    }
}