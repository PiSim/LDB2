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
        private CalibrationReport _selectedCalibration;
        private DBEntities _entities;
        private DBPrincipal _principal;
        private DelegateCommand _addCalibration, _addMaintenanceEvent;
        private EventAggregator _eventAggregator;
        private IInstrumentServiceProvider _instrumentServiceProvider;
        private Instrument _instance;

        public InstrumentEditViewModel(DBEntities entities, 
                                    DBPrincipal principal,
                                    EventAggregator aggregator,
                                    IInstrumentServiceProvider instrumentServiceProvider) : base()
        {
            _entities = entities;
            _eventAggregator = aggregator;
            _instrumentServiceProvider = instrumentServiceProvider;
            _principal = principal;

            _eventAggregator.GetEvent<CommitRequested>().Subscribe(() => _entities.SaveChanges());

            _addCalibration = new DelegateCommand(
                () =>
                {
                    _instrumentServiceProvider.RegisterNewCalibration(_instance);
                    RaisePropertyChanged("CalibrationReportList");
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

        public List<CalibrationReport> CalibrationReportList
        {
            get 
            { 
                if (_instance = null)
                    return new Linq<CalibrationReport>();
                    
                return new List<CalibrationReport>(_instance.CalibrationReports); 
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

        public Instrument InstrumentInstance
        {
            get { return _instance; }
            set
            {
                _instance = value;
                RaisePropertyChanged("Code");
                RaisePropertyChanged("CalibrationReportList");
            }
        }

        public CalibrationReport SelectedCalibration
        {
            get { return _selectedCalibration; }
            set
            {
                _selectedCalibration = value;
                RaisePropertyChanged("SelectedCalibration");
            }
        }
    }
}
