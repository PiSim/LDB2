using Infrastructure;
using Infrastructure.Events;
using Prism.Commands;
using Prism.Events;

namespace Controls.ViewModels
{
    public class ToolbarViewModel : Prism.Mvvm.BindableBase
    {
        #region Fields

        private IEventAggregator _eventAggregator;
        private DelegateCommand<string> _runSearch;

        #endregion Fields

        #region Constructors

        public ToolbarViewModel(IEventAggregator eventAggregator) : base()
        {
            _eventAggregator = eventAggregator;

            OpenCurrentUserMenuCommand = new DelegateCommand(
                () =>
                {
                    NavigationToken token = new NavigationToken(UserViewNames.CurrentUserMain);
                    _eventAggregator.GetEvent<NavigationRequested>().Publish(token);
                });

            NavigateBackCommand = new DelegateCommand(
                () =>
                {
                    _eventAggregator.GetEvent<NavigateBackRequested>().Publish();
                });

            NavigateForwardCommand = new DelegateCommand(
                () =>
                {
                    _eventAggregator.GetEvent<NavigateForwardRequested>().Publish();
                });

            RequestNavigationCommand = new DelegateCommand<IModuleNavigationTag>(
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

        #endregion Constructors

        #region Properties

        public DelegateCommand NavigateBackCommand { get; }

        public DelegateCommand NavigateForwardCommand { get; }

        public DelegateCommand OpenCurrentUserMenuCommand { get; }

        public DelegateCommand<IModuleNavigationTag> RequestNavigationCommand { get; }

        public DelegateCommand<string> RunSearchCommand => _runSearch;

        #endregion Properties
    }
}