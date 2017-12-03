using DBManager;
using DBManager.EntityExtensions;
using DBManager.Services;
using Infrastructure;
using Infrastructure.Events;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instruments.ViewModels
{
    public class InstrumentMainViewModel : BindableBase
    {
        private DBPrincipal _principal;
        private DelegateCommand _deleteInstrument, _newInstrument, _openInstrument,
                                _openPending;
        private EventAggregator _eventAggregator;
        private IDataService _dataService;
        private IInstrumentService _instrumentService;
        private Instrument _selectedInstrument,
                            _selectedPending;

        public InstrumentMainViewModel(DBPrincipal principal,
                                        EventAggregator eventAggregator,
                                        IDataService dataService,
                                        IInstrumentService instrumentService) : base()
        {
            _principal = principal;
            _eventAggregator = eventAggregator;
            _dataService = dataService;
            _instrumentService = instrumentService;

            _deleteInstrument = new DelegateCommand(
                () =>
                {
                    _selectedInstrument.Delete();
                    SelectedInstrument = null;
                },
                () => IsInstrumentAdmin && _selectedInstrument != null);

            _newInstrument = new DelegateCommand(
                () =>
                {
                    _instrumentService.CreateInstrument();
                },
                () => IsInstrumentAdmin);

            _openInstrument = new DelegateCommand(
                () =>
                {
                    NavigationToken token = new NavigationToken(InstrumentViewNames.InstrumentEditView,
                                                                SelectedInstrument);
                    _eventAggregator.GetEvent<NavigationRequested>().Publish(token);
                },
                () => SelectedInstrument != null);

            _openPending = new DelegateCommand(
                () =>
                {
                    NavigationToken token = new NavigationToken(InstrumentViewNames.InstrumentEditView,
                                                                SelectedPending);
                    _eventAggregator.GetEvent<NavigationRequested>().Publish(token);
                });

            _eventAggregator.GetEvent<CalibrationIssued>().Subscribe(
                calRep => 
                {
                    RaisePropertyChanged("PendingCalibrationsList");
                    RaisePropertyChanged("CalibrationsList");
                });

            _eventAggregator.GetEvent<InstrumentListUpdateRequested>().Subscribe(
                () =>
                {
                     RaisePropertyChanged("InstrumentList");
                     RaisePropertyChanged("PendingCalibrationsList");
                });

        }

        public IEnumerable<CalibrationReport> CalibrationsList => _dataService.GetCalibrationReports();

        public DelegateCommand DeleteInstrumentCommand
        {
            get { return _deleteInstrument; }
        }

        public IEnumerable<Instrument> InstrumentList => _dataService.GetInstruments();

        private bool IsInstrumentAdmin
        {
            get { return _principal.IsInRole(UserRoleNames.InstrumentAdmin); }
        }

        public DelegateCommand NewInstrumentCommand
        {
            get { return _newInstrument; }
        }

        public DelegateCommand OpenInstrumentCommand
        {
            get { return _openInstrument; }
        }

        public DelegateCommand OpenPendingCommand
        {
            get { return _openPending; }
        }

        public IEnumerable<Instrument> PendingCalibrationsList => _instrumentService.GetCalibrationCalendar();

        public Instrument SelectedInstrument
        {
            get { return _selectedInstrument; }
            set
            {
                _selectedInstrument = value;

                _openInstrument.RaiseCanExecuteChanged();
                _deleteInstrument.RaiseCanExecuteChanged();

                RaisePropertyChanged("SelectedInstrument");
            }
        }


        public Instrument SelectedPending
        {
            get { return _selectedPending; }
            set
            {
                _selectedPending = value;
                RaisePropertyChanged("SelectedPending");
            }
        }
    }
}
