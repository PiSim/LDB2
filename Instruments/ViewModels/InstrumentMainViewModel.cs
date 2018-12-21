using DataAccessCore;
using DataAccessCore.Commands;
using Infrastructure;
using Infrastructure.Events;
using Instruments.Commands;
using Instruments.Queries;
using LInst;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Instruments.ViewModels
{
    public class InstrumentMainViewModel : BindableBase
    {
        #region Fields

        private IEventAggregator _eventAggregator;
        private InstrumentService _instrumentService;
        private IDataService<LInstContext> _lInstData;

        private Instrument _selectedInstrument,
                            _selectedPending;

        #endregion Fields

        #region Constructors

        public InstrumentMainViewModel(IDataService<LInstContext> lInstData,
                                        IEventAggregator eventAggregator,
                                        InstrumentService instrumentService) : base()
        {
            _eventAggregator = eventAggregator;
            _lInstData = lInstData;
            _instrumentService = instrumentService;

            InstrumentList = _lInstData.RunQuery(new InstrumentsQuery()).ToList();

            DeleteInstrumentCommand = new DelegateCommand(
                () =>
                {
                    DialogResult confirmation = MessageBox.Show("Lo strumento selezionato verrà eliminato, continuare?",
                                                            "Conferma Eliminazione",
                                                            MessageBoxButtons.YesNo);
                    if (confirmation == DialogResult.Yes)
                    {
                        _lInstData.Execute(new DeleteEntityCommand<LInstContext>(_selectedInstrument));
                        SelectedInstrument = null;
                    }
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
                                                                _lInstData.RunQuery(new InstrumentQuery() { ID = SelectedInstrument.ID }));
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

            UpdateAllCalibrationStatusesCommand = new DelegateCommand(
                () =>
                {
                    _lInstData.Execute(new BulkUpdateInstrumentCalibrationStatusCommand(InstrumentList));
                    RefreshList();
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
                    RefreshList();
                    RaisePropertyChanged("PendingCalibrationsList");
                });
        }

        #endregion Constructors

        #region Methods

        private void RefreshList()
        {
            InstrumentList = _lInstData.RunQuery(new InstrumentsQuery()).ToList();
            RaisePropertyChanged("InstrumentList");
        }

        #endregion Methods

        #region Properties

        public IEnumerable<CalibrationReport> CalibrationsList => _lInstData.RunQuery(new CalibrationReportsQuery()).ToList();

        public DelegateCommand DeleteInstrumentCommand { get; }

        public IEnumerable<Instrument> InstrumentList { get; private set; }

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

        public DelegateCommand UpdateAllCalibrationStatusesCommand { get; set; }

        private bool IsInstrumentAdmin => Thread.CurrentPrincipal.IsInRole(UserRoleNames.InstrumentAdmin);

        #endregion Properties
    }
}