using Infrastructure.Events;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controls.ViewModels
{
    public class StatusBarViewModel : BindableBase
    {
        private EventAggregator _eventAggregator;
        private string _shownMessage;

        public StatusBarViewModel(EventAggregator eventAggregator) : base()
        {
            _eventAggregator = eventAggregator;
            
            _eventAggregator.GetEvent<StatusNotificationIssue>().Subscribe(
                msg => 
                {
                    ShownMessage = msg;
                }
            );
        }

        public string ShownMessage
        {
            get { return _shownMessage; }
            set
            {
                _shownMessage = value;
            }
        }

    }
}
