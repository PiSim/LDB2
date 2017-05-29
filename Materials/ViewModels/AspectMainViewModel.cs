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
        private DelegateCommand _createAspect, _removeAspect;
        private EventAggregator _eventAggregator;

        public AspectMainViewModel(EventAggregator eventAggregator) : base()
        {
            _eventAggregator = eventAggregator;

            _createAspect = new DelegateCommand(
                () =>
                {
                    
                },
                () => CanModify);

            _removeAspect = new DelegateCommand(
                () =>
                {
                    _selectedAspect.Delete();
                    SelectedAspect = null;
                    RaisePropertyChanged("AspectList");
                },
                () => _selectedAspect != null && CanModify);
        }

        public string AspectDetailRegionName
        {
            get { return RegionNames.AspectDetailRegion; }
        }

        public IEnumerable<Aspect> AspectList
        {
            get { return MaterialService.GetAspects(); }
        }

        public bool CanModify
        {
            get { return true; }
        }

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
