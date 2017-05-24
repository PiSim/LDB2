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
using System.Windows;

namespace Instruments.ViewModels
{
    public class InstrumentEditViewModel : BindableBase
    {
        private bool _editMode;
        private CalibrationReport _selectedCalibration;
        private DBPrincipal _principal;
        private DelegateCommand _addCalibration, _addMaintenanceEvent;
        private EventAggregator _eventAggregator;
        private Instrument _instance;

        public InstrumentEditViewModel(DBPrincipal principal,
                                        EventAggregator aggregator) : base()
        {
            _editMode = false;
            _eventAggregator = aggregator;
            _principal = principal;

            _eventAggregator.GetEvent<CommitRequested>().Subscribe(() => _instance.Update());

            _addCalibration = new DelegateCommand(
                () =>
                {
                    _eventAggregator.GetEvent<NewCalibrationRequested>()
                                    .Publish(_instance);
                });

            _addMaintenanceEvent = new DelegateCommand(
                () =>
                {
                    _eventAggregator.GetEvent<NewMaintenanceEventRequested>()
                                    .Publish(_instance);
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

        public IEnumerable<CalibrationReport> CalibrationReportList
        {
            get 
            { 
                if (_instance == null)
                    return new List<CalibrationReport>();
                    
                return _instance.CalibrationReports; 
            }
        }

        public Visibility CalibrationTabVisible
        {
            get
            {
                if (_instance == null)
                    return Visibility.Visible;

                else if (_instance.IsUnderControl)
                    return Visibility.Visible;

                else
                    return Visibility.Hidden;
            }
        }

        public bool CanModifyInstrumentInfo
        {
            get { return true; }
        }

        public bool EditMode
        {
            get { return _editMode; }
        }

        public string InstrumentCode
        {
            get
            {
                if (_instance == null)
                    return null;
                return _instance.Code;
            }
        }

        public string InstrumentDescription
        {
            get
            {
                if (_instance == null)
                    return null;

                else
                    return _instance.Description;
            }

            set
            {
                if (_instance == null)
                    return;

                _instance.Description = value;
            }
        }

        public Instrument InstrumentInstance
        {
            get { return _instance; }
            set
            {
                _instance = value;
                _instance.Load();

                RaisePropertyChanged("CalibrationReportList");
                RaisePropertyChanged("CalibrationTabVisible");
                RaisePropertyChanged("InstrumentCode");
                RaisePropertyChanged("InstrumentDescription");
                RaisePropertyChanged("InstrumentManufacturer");
                RaisePropertyChanged("InstrumentModel");
                RaisePropertyChanged("InstrumentSerialNumber");
                RaisePropertyChanged("InstrumentType");
            }
        }

        public string InstrumentManufacturer
        {
            get
            {
                if (_instance == null)
                    return null;

                else
                    return _instance.Manufacturer.Name;
            }
        }

        public string InstrumentModel
        {
            get
            {
                if (_instance == null)
                    return null;

                else
                    return _instance.Model;
            }
        }

        public string InstrumentSerialNumber
        {
            get
            {
                if (_instance == null)
                    return null;

                else
                    return _instance.SerialNumber;
            }
        }

        public string InstrumentType
        {
            get
            {
                if (_instance == null)
                    return null;

                else
                    return _instance.InstrumentType.Name;
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
