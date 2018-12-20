using Controls.Views;
using DataAccess;
using Infrastructure;
using Infrastructure.Commands;
using Infrastructure.Events;
using Infrastructure.Queries;
using LabDbContext;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Specifications.ViewModels
{
    public class MethodMainViewModel : BindableBase
    {
        #region Fields

        private IEventAggregator _eventAggregator;
        private IDataService<LabDbEntities> _labDbData;
        private Method _selectedMethod;
        private ISpecificationService _specificationService;

        #endregion Fields

        #region Constructors

        public MethodMainViewModel(IEventAggregator aggregator,
                                    IDataService<LabDbEntities> labDbData,
                                    ISpecificationService specificationService) : base()
        {
            _labDbData = labDbData;
            _eventAggregator = aggregator;
            _specificationService = specificationService;

            DeleteMethodCommand = new DelegateCommand(
                () =>
                {
                    _labDbData.Execute(new DeleteEntityCommand(_selectedMethod));
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

        #endregion Constructors

        #region Commands

        private DelegateCommand DeleteMethodCommand { get; }

        private DelegateCommand NewMethodCommand { get; }

        #endregion Commands

        #region Properties

        public IEnumerable<Method> MethodList => _labDbData.RunQuery(new MethodsQuery())
                                                            .ToList();

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

        private bool IsSpecAdmin => Thread.CurrentPrincipal.IsInRole(UserRoleNames.SpecificationAdmin);

        #endregion Properties
    }
}