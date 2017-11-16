using DBManager;
using DBManager.EntityExtensions;
using DBManager.Services;
using Infrastructure;
using Infrastructure.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Materials.ViewModels
{
    public class AspectMainViewModel : BindableBase
    {
        private Aspect _selectedAspect;
        private DBPrincipal _principal;
        private DelegateCommand _createAspect, _removeAspect;
        private EventAggregator _eventAggregator;
        private IEnumerable<Aspect> _aspectList;
        private readonly MaterialService _materialService;

        public AspectMainViewModel(DBPrincipal principal,
                                EventAggregator eventAggregator,
                                MaterialService materialService) : base()
        {
            _principal = principal;
            _eventAggregator = eventAggregator;
            _materialService = materialService;

            _createAspect = new DelegateCommand(
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

            _removeAspect = new DelegateCommand(
                () =>
                {
                    _selectedAspect.Delete();
                    SelectedAspect = null;
                    RaisePropertyChanged("AspectList");
                },
                () => false);
        }

        public string AspectDetailRegionName
        {
            get { return RegionNames.AspectDetailRegion; }
        }

        public IEnumerable<Aspect> AspectList
        {
            get
            {
                if (_aspectList == null)
                    _aspectList = DBManager.Services.MaterialService.GetAspects();

                return _aspectList;
            }
        }

        public bool CanModify
        {
            get { return _principal.IsInRole(UserRoleNames.MaterialEdit); }
        }

        public DelegateCommand CreateAspectCommand => _createAspect;

        public DelegateCommand RemoveAspectCommand
        {
            get { return _removeAspect; }
        }

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
    }
}
