using DBManager;
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
    public class ExternalConstructionMainViewModel : BindableBase
    {
        private DBPrincipal _principal;
        private DelegateCommand _createExternalConstruction, _removeExternalConstruction;
        private EventAggregator _eventAggregator;
        private ExternalConstruction _selectedExternalConstruction;

        public ExternalConstructionMainViewModel(DBPrincipal principal,
                                                EventAggregator eventAggregator) : base()
        {
            _eventAggregator = eventAggregator;
            _principal = principal;

            _createExternalConstruction = new DelegateCommand(
                () =>
                {
                    if (MaterialService.CreateNewExternalConstruction() != null)
                        RaisePropertyChanged("ExternalConstructionList");
                },
                () => _principal.IsInRole(UserRoleNames.MaterialEdit));

            _removeExternalConstruction = new DelegateCommand(
                () =>
                {
                    _selectedExternalConstruction.Delete();

                    RaisePropertyChanged("ExternalConstructionList");
                },
                () => _selectedExternalConstruction != null
                    && _principal.IsInRole(UserRoleNames.MaterialAdmin));

            #region Event Subscriptions

            _eventAggregator.GetEvent<ExternalConstructionChanged>()
                            .Subscribe(ect => RaisePropertyChanged("ExternalConstructionList"));

            #endregion
        }

        public DelegateCommand CreateExternalConstructionCommand
        {
            get { return _createExternalConstruction; }
        }

        public string ExternalConstructionDetailRegionName
        {
            get { return RegionNames.ExternalConstructionDetailRegion; }
        }

        public IEnumerable<ExternalConstruction> ExternalConstructionList
        {
            get { return DBManager.Services.MaterialService.GetExternalConstructions(); }
        }

        public DelegateCommand RemoveExternalConstructionCommand
        {
            get { return _removeExternalConstruction; }
        }

        public ExternalConstruction SelectedExternalConstruction
        {
            get { return _selectedExternalConstruction; }
            set
            {
                _selectedExternalConstruction = value;
                _removeExternalConstruction.RaiseCanExecuteChanged();

                NavigationToken token = new NavigationToken(MaterialViewNames.ExternalConstructionDetail,
                                                            _selectedExternalConstruction,
                                                            RegionNames.ExternalConstructionDetailRegion);

                _eventAggregator.GetEvent<NavigationRequested>().Publish(token);

                RaisePropertyChanged("SelectedExternalConstruction");
            }
        }
    }
}
