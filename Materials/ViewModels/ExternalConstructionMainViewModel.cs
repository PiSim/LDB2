using Controls.Views;
using DataAccess;
using Infrastructure;
using Infrastructure.Events;
using LabDbContext;
using LabDbContext.Services;
using Materials.Queries;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Materials.ViewModels
{
    public class ExternalConstructionMainViewModel : BindableBase
    {
        #region Fields

        private IEventAggregator _eventAggregator;
        private IDataService<LabDbEntities> _labDbData;
        private MaterialService _materialService;
        private ExternalConstruction _selectedExternalConstruction;

        #endregion Fields

        #region Constructors

        public ExternalConstructionMainViewModel(IDataService<LabDbEntities> labDbData,
                                                IEventAggregator eventAggregator,
                                                MaterialService materialService) : base()
        {
            _labDbData = labDbData;
            _materialService = materialService;
            _eventAggregator = eventAggregator;

            CreateExternalConstructionCommand = new DelegateCommand(
                () =>
                {
                    if (_materialService.CreateNewExternalConstruction() != null)
                        RaisePropertyChanged("ExternalConstructionList");
                },
                () => Thread.CurrentPrincipal.IsInRole(UserRoleNames.MaterialEdit));

            RemoveExternalConstructionCommand = new DelegateCommand(
                () =>
                {
                    _selectedExternalConstruction.Delete();

                    RaisePropertyChanged("ExternalConstructionList");
                },
                () => _selectedExternalConstruction != null
                    && Thread.CurrentPrincipal.IsInRole(UserRoleNames.MaterialAdmin));

            #region Event Subscriptions

            _eventAggregator.GetEvent<ExternalConstructionChanged>()
                            .Subscribe(ect => RaisePropertyChanged("ExternalConstructionList"));

            #endregion Event Subscriptions
        }

        #endregion Constructors

        #region Properties

        public DelegateCommand CreateExternalConstructionCommand { get; }

        public string ExternalConstructionDetailRegionName => RegionNames.ExternalConstructionDetailRegion;

        public IEnumerable<ExternalConstruction> ExternalConstructionList => _labDbData.RunQuery(new ExternalConstructionsQuery()).ToList();

        public DelegateCommand RemoveExternalConstructionCommand { get; }

        public ExternalConstruction SelectedExternalConstruction
        {
            get { return _selectedExternalConstruction; }
            set
            {
                _selectedExternalConstruction = value;
                RemoveExternalConstructionCommand.RaiseCanExecuteChanged();

                NavigationToken token = new NavigationToken(MaterialViewNames.ExternalConstructionDetail,
                                                            _selectedExternalConstruction,
                                                            RegionNames.ExternalConstructionDetailRegion);

                _eventAggregator.GetEvent<NavigationRequested>().Publish(token);

                RaisePropertyChanged("SelectedExternalConstruction");
            }
        }

        #endregion Properties
    }
}