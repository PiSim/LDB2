using Infrastructure.Events;
using Prism.Events;
using Prism.Mvvm;

namespace Controls.ViewModels
{
    public class StatusBarViewModel : BindableBase
    {
        #region Fields

        private IEventAggregator _eventAggregator;
        private string _shownMessage;

        #endregion Fields

        #region Constructors

        public StatusBarViewModel(IEventAggregator eventAggregator) : base()
        {
            _eventAggregator = eventAggregator;

            _eventAggregator.GetEvent<StatusNotificationIssued>().Subscribe(
                msg =>
                {
                    ShownMessage = msg;
                }
            );
        }

        #endregion Constructors

        #region Properties

        public string ShownMessage
        {
            get { return _shownMessage; }
            set
            {
                _shownMessage = value;
                RaisePropertyChanged("ShownMessage");
            }
        }

        #endregion Properties
    }
}