using Infrastructure;
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
        private DelegateCommand<IModuleNavigationTag> _requestNavigation;
        private IEventAggregator _eventAggregator;

        public ToolbarViewModel(IEventAggregator eventAggregator) : base()
        {
            _eventAggregator = eventAggregator;
            _requestNavigation = new DelegateCommand<IModuleNavigationTag>(
                view => 
                {
                    _eventAggregator.GetEvent<NavigationRequested>().Publish(view.ViewName);
                });
        }

        public DelegateCommand<IModuleNavigationTag> RequestNavigationCommand
        {
            get { return _requestNavigation; }
        }


    }
}
