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

namespace Instruments.ViewModels
{
    public class InstrumentEditViewModel : BindableBase
    {
        private DBEntities _entities;
        private DelegateCommand _addCalibration, _addMaintenanceEvent;
        private EventAggregator _eventAggregator;
        private IInstrumentServiceProvider _instrumentServiceProvider;
        private Instrument _instance;

        public InstrumentEditViewModel(DBEntities entities, 
                                    EventAggregator aggregator,
                                    IInstrumentServiceProvider instrumentServiceProvider) : base()
        {
            _entities = entities;
            _eventAggregator = aggregator;
            _instrumentServiceProvider = instrumentServiceProvider;

            _eventAggregator.GetEvent<CommitRequested>().Subscribe(() => _entities.SaveChanges());

            _addCalibration = new DelegateCommand(
                () =>
                {
                    _instrumentServiceProvider.RegisterNewCalibration(_instance);
                });

            _addMaintenanceEvent = new DelegateCommand(
                () =>
                {
                    _instrumentServiceProvider.AddMaintenanceEvent(_instance);
                });
        }

        public DelegateCommand AddCalibrationCommand
        {
            get { return _addCalibration; }
        }

        public DelegateCommand AddMaintenanceEvent
        {
            get { return _addMaintenanceEvent; }
        }

        public Instrument InstrumentInstance
        {
            get { return _instance; }
            set
            {
                _instance = value;
                OnPropertyChanged("Code");
            }
        }

        public string Code
        {
            get
            {
                if (_instance == null)
                    return null;
                return _instance.Code;
            }
        }
    }
}
