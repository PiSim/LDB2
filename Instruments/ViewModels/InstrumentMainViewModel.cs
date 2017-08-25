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
        private DelegateCommand _deleteInstrument, _newInstrument, _openInstrument;
        private EventAggregator _eventAggregator;
        private Instrument _selectedInstrument;

        public InstrumentMainViewModel(DBPrincipal principal,
                                        EventAggregator eventAggregator) : base()
        {
            _principal = principal;
            _eventAggregator = eventAggregator;

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
                    _eventAggregator.GetEvent<InstrumentCreationRequested>()
                                    .Publish();
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

        public IEnumerable<CalibrationReport> CalibrationsList
        {
            get { return InstrumentService.GetCalibrationReports(); }
        }

        public DelegateCommand DeleteInstrumentCommand
        {
            get { return _deleteInstrument; }
        }

        public IEnumerable<Instrument> InstrumentList
        {
            get { return InstrumentService.GetInstruments(); }
        }

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

        public IEnumerable<Instrument> PendingCalibrationsList
        {
            get
            {
                return InstrumentService.GetCalibrationCalendar();
            }
        }

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

    }
}
