using DBManager;
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
        private DBEntities _entities;
        private EventAggregator _eventAggregator;
        private Instrument _selectedInstrument;
        private IInstrumentServiceProvider _instrumentServiceProvider;

        public InstrumentMainViewModel(EventAggregator eventAggregator,
                                        DBEntities entities,
                                        IInstrumentServiceProvider instrumentServiceProvider) : base()
        {
            _entities = entities;
            _eventAggregator = eventAggregator;
            _instrumentServiceProvider = instrumentServiceProvider;
            
            _newInstrument = new DelegateCommand(
                () =>
                {
                    _instrumentServiceProvider.RegisterNewInstrument();
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
                calRep => RaisePropertyChanged("PendingCalibrationsList"));

            _eventAggregator.GetEvent<InstrumentListUpdateRequested>().Subscribe(
                () => RaisePropertyChanged("InstrumentList"));

        }

        public List<Instrument> InstrumentList
        {
            get { return new List<Instrument>(_entities.Instruments); }
        }

        public DelegateCommand NewInstrumentCommand
        {
            get { return _newInstrument; }
        }

        public DelegateCommand OpenInstrumentCommand
        {
            get { return _openInstrument; }
        }

        public List<Instrument> PendingCalibrationsList
        {
            get
            {
                return new List<Instrument>(_entities.Instruments.Where(inst => inst.IsUnderControl).OrderBy(inst => inst.CalibrationDueDate));
            }
        }

        public Instrument SelectedInstrument
        {
            get { return _selectedInstrument; }
            set
            {
                _selectedInstrument = value;
                _openInstrument.RaiseCanExecuteChanged();
            }
        }

    }
}
