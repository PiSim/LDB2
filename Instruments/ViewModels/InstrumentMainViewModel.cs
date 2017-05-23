using DBManager;
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
        private DelegateCommand _newInstrument, _openInstrument;
        private EventAggregator _eventAggregator;
        private Instrument _selectedInstrument;

        public InstrumentMainViewModel(EventAggregator eventAggregator) : base()
        {
            _eventAggregator = eventAggregator;
            
            _newInstrument = new DelegateCommand(
                () =>
                {
                    _eventAggregator.GetEvent<InstrumentCreationRequested>()
                                    .Publish();
                });

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
                });

            _eventAggregator.GetEvent<InstrumentListUpdateRequested>().Subscribe(
                () =>
                {
                     RaisePropertyChanged("InstrumentList");
                     RaisePropertyChanged("PendingCalibrationsList");
                });

        }

        public IEnumerable<Instrument> InstrumentList
        {
            get { return InstrumentService.GetInstruments(); }
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
                RaisePropertyChanged("SelectedInstrument");
            }
        }

    }
}
