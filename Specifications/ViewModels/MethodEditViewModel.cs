using DBManager;
using Infrastructure.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 
namespace Specifications.ViewModels
{
    public class MethodEditViewModel : BindableBase
    {
        private DBEntities _entities;
        private DelegateCommand _addMeasurement, _removeMeasurement;
        private EventAggregator _eventAggregator;
        private Method _methodInstance;
        private MethodMeasurement _selectedMeasurement;
        private ObservableCollection<MethodMeasurement>_measurementList;

        public MethodEditViewModel(DBEntities entities, EventAggregator aggregator) : base()
        {
            _entities = entities;
            _eventAggregator = aggregator;

            _eventAggregator.GetEvent<CommitRequested>().Subscribe(() => _entities.SaveChanges());
            
            _addMeasurement = new DelegateCommand(
                () => 
                {
                    MethodMeasurement tempMea = new MethodMeasurement();
                    tempMea.Name = "Nuova Prova";
                    tempMea.UM = "";
                    Measurements.Add(tempMea);
                });
            
            _removeMeasurement = new DelegateCommand(
                () => 
                {
                    Measurements.Remove(SelectedMeasurement);
                    SelectedMeasurement = null;
                },
                () => SelectedMeasurement != null );
        }
        
        public DelegateCommand AddMeasurementCommand
        {
            get { return _addMeasurement; }
        }

        public Method MethodInstance
        {
            get { return _methodInstance; }
            set
            {
                _methodInstance = value;
                Measurements = new ObservableCollection<MethodMeasurement>(_methodInstance.Measurements);
                OnPropertyChanged("Name");
            }
        }

        public ObservableCollection<MethodMeasurement> Measurements
        {
            get 
            { 
                return _measurementList;
            }
            
            private set 
            {
                _measurementList = value;
                OnPropertyChanged("Measurements");
            }
        }

        public string Name
        {
            get
            {
                return (_methodInstance != null) ? _methodInstance.Standard.Name : null;
            }
        }
        
        public DelegateCommand RemoveMeasurementCommand
        {
            get { return _removeMeasurement; }
        }
        
        public MethodMeasurement SelectedMeasurement
        {
            get 
            {
                return _selectedMeasurement;
            }
            set
            {
                _selectedMeasurement = value;
                _removeMeasurement.RaiseCanExecuteChanged();
                OnPropertyChanged("SelectedMeasurement");
            }
        }
        
    }
}
