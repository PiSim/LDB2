using DataAccess;
using Infrastructure;
using Infrastructure.Events;
using Instruments.Queries;
using LabDbContext;
using LabDbContext.EntityExtensions;
using LabDbContext.Services;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Instruments.ViewModels
{
    public class InstrumentMainViewModel : BindableBase
    {
        #region Fields

        private IEventAggregator _eventAggregator;
        private InstrumentService _instrumentService;
        private IDataService<LabDbEntities> _labDbData;

        private Instrument _selectedInstrument,
                            _selectedPending;

        #endregion Fields

        #region Constructors

        public InstrumentMainViewModel(IDataService<LabDbEntities> labDbData,
                                        IEventAggregator eventAggregator,
                                        InstrumentService instrumentService) : base()
        {
            _eventAggregator = eventAggregator;
            _labDbData = labDbData;
            _instrumentService = instrumentService;

            DeleteInstrumentCommand = new DelegateCommand(
                () =>
                {
                    _selectedInstrument.Delete();
                    SelectedInstrument = null;
                },
                () => IsInstrumentAdmin && _selectedInstrument != null);

            NewInstrumentCommand = new DelegateCommand(
                () =>
                {
                    _instrumentService.CreateInstrument();
                },
                () => IsInstrumentAdmin);

            OpenInstrumentCommand = new DelegateCommand(
                () =>
                {
                    NavigationToken token = new NavigationToken(InstrumentViewNames.InstrumentEditView,
                                                                SelectedInstrument);
                    _eventAggregator.GetEvent<NavigationRequested>().Publish(token);
                },
                () => SelectedInstrument != null);

            OpenPendingCommand = new DelegateCommand(
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

        #endregion Constructors

        #region Properties

        public IEnumerable<CalibrationReport> CalibrationsList => _labDbData.RunQuery(new CalibrationReportsQuery()).ToList();

        public DelegateCommand DeleteInstrumentCommand { get; }

        public IEnumerable<Instrument> InstrumentList => _labDbData.RunQuery(new InstrumentsQuery()).ToList();

        public DelegateCommand NewInstrumentCommand { get; }
        public DelegateCommand OpenInstrumentCommand { get; }
        public DelegateCommand OpenPendingCommand { get; }
        public IEnumerable<Instrument> PendingCalibrationsList => _instrumentService.GetCalibrationCalendar();

        public Instrument SelectedInstrument
        {
            get { return _selectedInstrument; }
            set
            {
                _selectedInstrument = value;

                OpenInstrumentCommand.RaiseCanExecuteChanged();
                DeleteInstrumentCommand.RaiseCanExecuteChanged();

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

        private bool IsInstrumentAdmin => Thread.CurrentPrincipal.IsInRole(UserRoleNames.InstrumentAdmin);

        #endregion Properties
    }
}