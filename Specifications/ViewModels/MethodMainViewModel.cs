using DBManager;
using DBManager.EntityExtensions;
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
        private DBPrincipal _principal;
        private DelegateCommand _deleteMethod, _newMethod;
        private EventAggregator _eventAggregator;
        private Method _selectedMethod;

        public MethodMainViewModel(DBPrincipal principal,
                                    EventAggregator aggregator) : base()
        {
            _eventAggregator = aggregator;
            _principal = principal;

            _deleteMethod = new DelegateCommand(
                () =>
                {
                    _selectedMethod.Delete();
                },
                () => IsSpecAdmin && _selectedMethod != null);

            _newMethod = new DelegateCommand(
                () =>
                {
                    _eventAggregator.GetEvent<MethodCreationRequested>().Publish();
                },
                () => IsSpecAdmin);

            _eventAggregator.GetEvent<MethodListUpdateRequested>().Subscribe(
                () => RaisePropertyChanged("MethodList"));
        }

        private DelegateCommand DeleteMethodCommand
        {
            get { return _deleteMethod; }
        }

        private bool IsSpecAdmin
        {
            get { return _principal.IsInRole(UserRoleNames.SpecificationAdmin); }
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
                _deleteMethod.RaiseCanExecuteChanged();
                RaisePropertyChanged("SelectedMethod");

                NavigationToken token = new NavigationToken(SpecificationViewNames.MethodEdit,
                                                            _selectedMethod,
                                                            RegionNames.MethodEditRegion);

                _eventAggregator.GetEvent<NavigationRequested>().Publish(token);
            }
        }
    }
}
