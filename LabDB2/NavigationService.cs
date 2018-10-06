using Controls.Views;
using Infrastructure.Events;
using Prism.Events;
using Prism.Regions;
using System;

namespace LabDB2
{
    public class NavigationService
    {
        #region Fields

        private IEventAggregator _eventAggregator;
        private IRegionManager _regionManager;

        #endregion Fields

        #region Constructors

        public NavigationService(IEventAggregator eventAggregator,
                                   IRegionManager regionManager)
        {
            _eventAggregator = eventAggregator;
            _regionManager = regionManager;

            _eventAggregator.GetEvent<NavigateBackRequested>().Subscribe(() => OnNavigateBackRequested(), true);
            _eventAggregator.GetEvent<NavigateForwardRequested>().Subscribe(() => OnNavigateForwardRequested(), true);
            _eventAggregator.GetEvent<NavigationRequested>().Subscribe(tkn => OnNavigationRequested(tkn), true);
        }

        #endregion Constructors

        #region Methods

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

            string regionName = (token.RegionName == null) ? RegionNames.MainRegion : token.RegionName;

            _regionManager.RequestNavigate(
                regionName,
                new Uri(token.ViewName, UriKind.Relative),
                parameters
                );
        }

        #endregion Methods
    }
}