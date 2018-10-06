using Controls.Views;
using DataAccess;
using Infrastructure;
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
    public class SpecificationMainViewModel : BindableBase
    {
        #region Fields

        private readonly IDataService<LabDbEntities> _labDbData;
        private readonly ISpecificationService _specificationService;
        private IEventAggregator _eventAggregator;
        private Specification _selectedSpecification;

        #endregion Fields

        #region Constructors

        public SpecificationMainViewModel(IEventAggregator aggregator,
                                            IDataService<LabDbEntities> labDbData,
                                            ISpecificationService specificationService)
            : base()
        {
            _labDbData = labDbData;
            _eventAggregator = aggregator;
            _specificationService = specificationService;

            NewSpecificationCommand = new DelegateCommand(
                () =>
                {
                    _specificationService.CreateSpecification();
                },
                () => Thread.CurrentPrincipal.IsInRole(UserRoleNames.SpecificationEdit));

            OpenSpecificationCommand = new DelegateCommand<Specification>(
                spec =>
                {
                    NavigationToken token = new NavigationToken(SpecificationViewNames.SpecificationEdit,
                                                                SelectedSpecification);
                    _eventAggregator.GetEvent<NavigationRequested>().Publish(token);
                });

            #region EventSubscriptions

            _eventAggregator.GetEvent<SpecificationChanged>()
                            .Subscribe(token =>
                            {
                                RaisePropertyChanged("SpecificationList");
                            });

            #endregion EventSubscriptions
        }

        #endregion Constructors

        #region Properties

        public DelegateCommand NewSpecificationCommand { get; }

        public DelegateCommand<Specification> OpenSpecificationCommand { get; }

        public Specification SelectedSpecification
        {
            get { return _selectedSpecification; }
            set
            {
                _selectedSpecification = value;
            }
        }

        public IEnumerable<Specification> SpecificationList => _labDbData.RunQuery(new SpecificationsQuery()).ToList();

        public string SpecificationMainListRegionName => RegionNames.SpecificationMainListRegion;

        #endregion Properties
    }
}