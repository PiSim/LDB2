using Infrastructure;
using Infrastructure.Events;
using Prism.Events;
using Prism.Regions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Navigation
{
    public class NavigationServiceProvider
    {
        private EventAggregator _eventAggregator;
        private IRegionManager _regionManager;

        public NavigationServiceProvider(EventAggregator eventAggregator,
                                        IRegionManager regionManager)
        {
            _eventAggregator = eventAggregator;
            _regionManager = regionManager;

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

        public void OnNavigationRequested(NavigationToken token)
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
