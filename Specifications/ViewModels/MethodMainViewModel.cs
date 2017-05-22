using DBManager;
using DBManager.Services;
using Infrastructure;
using Infrastructure.Events;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Specifications.ViewModels
{
    public class MethodMainViewModel : BindableBase
    {
        private DelegateCommand _newMethod;
        private EventAggregator _eventAggregator;
        private Method _selectedMethod;
        private UnityContainer _container;

        public MethodMainViewModel(EventAggregator aggregator,
                                    UnityContainer container) : base()
        {
            _container = container;
            _eventAggregator = aggregator;

            _newMethod = new DelegateCommand(
                () =>
                {
                    _eventAggregator.GetEvent<MethodCreationRequested>().Publish();
                });

            _eventAggregator.GetEvent<MethodListUpdateRequested>().Subscribe(
                () => RaisePropertyChanged("MethodList"));
        }

        public IEnumerable<Method> MethodList
        {
            get
            {
                return SpecificationService.GetMethods();
            }
        }


        public DelegateCommand NewMethodCommand
        {
            get { return _newMethod; }
        }

        public Method SelectedMethod
        {
            get
            {
                return _selectedMethod;
            }

            set
            {
                _selectedMethod = value;
                RaisePropertyChanged("SelectedMethod");
                NavigationToken token = new NavigationToken(ViewNames.MethodEditView,
                                                            _selectedMethod,
                                                            RegionNames.MethodEditRegion);

                _eventAggregator.GetEvent<NavigationRequested>().Publish(token);
            }
        }
    }
}
