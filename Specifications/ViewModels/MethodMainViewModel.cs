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
        private EventAggregator _eventAggregator;
        private readonly IDataService _dataService;
        private ISpecificationService _specificationService;
        private Method _selectedMethod;

        public MethodMainViewModel(DBPrincipal principal,
                                    EventAggregator aggregator,
                                    IDataService dataService,
                                    ISpecificationService specificationService) : base()
        {
            _dataService = dataService;
            _eventAggregator = aggregator;
            _principal = principal;
            _specificationService = specificationService;

            DeleteMethodCommand = new DelegateCommand(
                () =>
                {
                    _selectedMethod.Delete();
                },
                () => IsSpecAdmin && _selectedMethod != null);

            NewMethodCommand = new DelegateCommand(
                () =>
                {
                    _specificationService.CreateMethod();
                },
                () => IsSpecAdmin);

            _eventAggregator.GetEvent<MethodChanged>().Subscribe(
                tkn => RaisePropertyChanged("MethodList"));
        }

        #region Commands

        private DelegateCommand DeleteMethodCommand { get; }

        private DelegateCommand NewMethodCommand { get; }

        #endregion

        private bool IsSpecAdmin
        {
            get { return _principal.IsInRole(UserRoleNames.SpecificationAdmin); }
        }

        public IEnumerable<Method> MethodList => _dataService.GetMethods();
        
        public Method SelectedMethod
        {
            get
            {
                return _selectedMethod;
            }

            set
            {
                _selectedMethod = value;
                DeleteMethodCommand.RaiseCanExecuteChanged();
                RaisePropertyChanged("SelectedMethod");

                NavigationToken token = new NavigationToken(SpecificationViewNames.MethodEdit,
                                                            _selectedMethod,
                                                            RegionNames.MethodEditRegion);

                _eventAggregator.GetEvent<NavigationRequested>().Publish(token);
            }
        }
    }
}
