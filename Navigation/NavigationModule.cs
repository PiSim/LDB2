﻿using Infrastructure;
using Infrastructure.Events;
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
            _eventAggregator.GetEvent<NavigateBackRequested>().Subscribe(() => OnNavigateBackRequested(), true);
            _eventAggregator.GetEvent<NavigateForwardRequested>().Subscribe(() => OnNavigateForwardRequested(), true);
            _eventAggregator.GetEvent<NavigationRequested>().Subscribe(tkn => OnNavigationRequested(tkn), true);
        }

        public void OnNavigateBackRequested()
        {
            var mainregion = _regionManager.Regions[RegionNames.MainRegion];
            mainregion.NavigationService.Journal.GoBack();
        }

        public void OnNavigateForwardRequested()
        {
            var mainregion = _regionManager.Regions[RegionNames.MainRegion];
            mainregion.NavigationService.Journal.GoForward();
        }

        public void OnNavigationRequested(string viewName)
        {
            _regionManager.RequestNavigate("MainRegion", new Uri(viewName, UriKind.Relative));
        }

        public void OnObjectVisualizationRequested(NavigationToken token)
        {
            NavigationParameters parameters = new NavigationParameters();
            parameters.Add("ObjectInstance", token.ObjectInstance);
            _regionManager.RequestNavigate(
                token.RegionName,
                new Uri(token.ViewName, UriKind.Relative),
                parameters
                );
        }
    }
}