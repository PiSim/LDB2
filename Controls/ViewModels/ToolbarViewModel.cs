using Infrastructure;
using Infrastructure.Events;
using Navigation;
using Prism;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controls.ViewModels
{
    class ToolbarViewModel : Prism.Mvvm.BindableBase
    {
        private DelegateCommand _navigateBack, _navigateForward, _openCurrentUserMenu;
        private DelegateCommand<IModuleNavigationTag> _requestNavigation;
        private DelegateCommand<string> _runSearch;
        private IEventAggregator _eventAggregator;

        public ToolbarViewModel(IEventAggregator eventAggregator) : base()
        {
            _eventAggregator = eventAggregator;

            _openCurrentUserMenu = new DelegateCommand(
                () =>
                {
                    NavigationToken token = new NavigationToken(UserViewNames.CurrentUserMain);
                    _eventAggregator.GetEvent<NavigationRequested>().Publish(token);
                });

            _navigateBack = new DelegateCommand(
                () =>
                {
                    _eventAggregator.GetEvent<NavigateBackRequested>().Publish();
                });

            _navigateForward = new DelegateCommand(
                () =>
                {
                    _eventAggregator.GetEvent<NavigateForwardRequested>().Publish();
                });

            _requestNavigation = new DelegateCommand<IModuleNavigationTag>(
                view => 
                {
                    NavigationToken token = new NavigationToken(view.ViewName);
                    _eventAggregator.GetEvent<NavigationRequested>().Publish(token);
                });

            _runSearch = new DelegateCommand<string>(
                sString =>
                {
                    _eventAggregator.GetEvent<BatchVisualizationRequested>()
                                    .Publish(sString);
                });    
        }

        public DelegateCommand NavigateBackCommand
        {
            get { return _navigateBack; }
        }

        public DelegateCommand NavigateForwardCommand
        {
            get { return _navigateForward; }
        }

        public DelegateCommand OpenCurrentUserMenuCommand
        {
            get { return _openCurrentUserMenu; }
        }

        public DelegateCommand<IModuleNavigationTag> RequestNavigationCommand
        {
            get { return _requestNavigation; }
        }
        
        public DelegateCommand<string> RunSearchCommand
        {
            get { return _runSearch; }
        }
    }
}
