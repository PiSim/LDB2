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
                    if (MaterialServiceProvider.CreateNewExternalConstruction() != null)
                        RaisePropertyChanged("ExternalConstructionList");
                },
                () => _principal.IsInRole(UserRoleNames.MaterialAdmin));

            _removeExternalConstruction = new DelegateCommand(
                () =>
                {
                    _selectedExternalConstruction.Delete();

                    RaisePropertyChanged("ExternalConstructionList");
                },
                () => _selectedExternalConstruction != null
                    && _principal.IsInRole(UserRoleNames.MaterialAdmin));


            _eventAggregator.GetEvent<ExternalConstructionModified>().Subscribe(
                () => RaisePropertyChanged("ExternalConstructionList"));
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
            get { return MaterialService.GetExternalConstructions(); }
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
