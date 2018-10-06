using Controls.Views;
using DataAccess;
using Infrastructure;
using Infrastructure.Events;
using LabDbContext;
using LabDbContext.EntityExtensions;
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
    public class AspectMainViewModel : BindableBase
    {
        #region Fields

        private readonly IDataService<LabDbEntities> _labDbData;
        private readonly MaterialService _materialService;
        private IEnumerable<Aspect> _aspectList;
        private IEventAggregator _eventAggregator;
        private Aspect _selectedAspect;

        #endregion Fields

        #region Constructors

        public AspectMainViewModel(IEventAggregator eventAggregator,
                                IDataService<LabDbEntities> labDbData,
                                MaterialService materialService) : base()
        {
            _labDbData = labDbData;
            _eventAggregator = eventAggregator;
            _materialService = materialService;

            CreateAspectCommand = new DelegateCommand(
                () =>
                {
                    Aspect tempAspect = _materialService.CreateAspect();
                    if (tempAspect != null)
                        _eventAggregator.GetEvent<AspectChanged>()
                                        .Publish(new EntityChangedToken(tempAspect,
                                                                        EntityChangedToken.EntityChangedAction.Created));

                    _aspectList = null;
                    RaisePropertyChanged("AspectList");
                },
                () => CanModify);

            RemoveAspectCommand = new DelegateCommand(
                () =>
                {
                    _selectedAspect.Delete();
                    SelectedAspect = null;
                    RaisePropertyChanged("AspectList");
                },
                () => false);
        }

        #endregion Constructors

        #region Properties

        public string AspectDetailRegionName => RegionNames.AspectDetailRegion;

        public IEnumerable<Aspect> AspectList
        {
            get
            {
                if (_aspectList == null)
                    _aspectList = _labDbData.RunQuery(new AspectsQuery()).ToList();

                return _aspectList;
            }
        }

        public bool CanModify => Thread.CurrentPrincipal.IsInRole(UserRoleNames.MaterialEdit);

        public DelegateCommand CreateAspectCommand { get; }

        public DelegateCommand RemoveAspectCommand { get; }

        public Aspect SelectedAspect
        {
            get { return _selectedAspect; }
            set
            {
                _selectedAspect = value;
                RaisePropertyChanged();
                NavigationToken token = new NavigationToken(MaterialViewNames.AspectDetail,
                                                            _selectedAspect,
                                                            AspectDetailRegionName);

                _eventAggregator.GetEvent<NavigationRequested>().Publish(token);
            }
        }

        #endregion Properties
    }
}