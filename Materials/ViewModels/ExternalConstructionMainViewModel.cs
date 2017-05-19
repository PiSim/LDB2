using DBManager;
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
        private DBEntities _entities;
        private DelegateCommand _createExternalConstruction, _removeExternalConstruction;
        private EventAggregator _eventAggregator;
        private ExternalConstruction _selectedExternalConstruction;

        public ExternalConstructionMainViewModel(DBEntities entities,
                                                EventAggregator eventAggregator) : base()
        {
            _entities = entities;
            _eventAggregator = eventAggregator;

            _createExternalConstruction = new DelegateCommand(
                () =>
                {
                    ExternalConstruction newEntry = new ExternalConstruction();
                    int nameCounter = 1;
                    string curName = "Nuova Construction";
                    while (true)
                    {
                        if (!_entities.ExternalConstructions.Any(exc => exc.Name == curName))
                            break;

                        else
                            curName = "Nuova Construction " + nameCounter++; 
                    }
                    newEntry.Name = curName;

                    _entities.ExternalConstructions.Add(newEntry);
                    _entities.SaveChanges();
                    RaisePropertyChanged("ExternalConstructionList");
                });

            _removeExternalConstruction = new DelegateCommand(
                () =>
                {
                    _entities.ExternalConstructions.Remove(_selectedExternalConstruction);
                    _entities.SaveChanges();

                    RaisePropertyChanged("ExternalConstructionList");
                },
                () => _selectedExternalConstruction != null);


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

        public List<ExternalConstruction> ExternalConstructionList
        {
            get { return new List<ExternalConstruction>(_entities.ExternalConstructions.OrderBy(exc => exc.Name)); }
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
