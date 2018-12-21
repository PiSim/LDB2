using Controls.Views;
using DataAccessCore;
using DataAccessCore.Commands;
using Infrastructure;
using Infrastructure.Events;
using Infrastructure.Queries;
using Instruments.Queries;
using LInst;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Instruments.ViewModels
{
    public class InstrumentEditViewModel : BindableBase
    {
        #region Fields

        private bool _editMode;
        private IEventAggregator _eventAggregator;
        private Instrument _instance;
        private InstrumentService _instrumentService;
        private IDataService<LInstContext> _lInstData;
        private InstrumentUtilizationArea _selectedArea;
        private CalibrationReport _selectedCalibration;
        private InstrumentMaintenanceEvent _selectedEvent;
        private InstrumentFile _selectedFile;

        #endregion Fields

        #region Constructors

        public InstrumentEditViewModel(IDataService<LInstContext> lInstData,
                                        IEventAggregator aggregator,
                                        InstrumentService instrumentService) : base()
        {
            _lInstData = lInstData;
            _editMode = false;
            _eventAggregator = aggregator;
            _instrumentService = instrumentService;

            AreaList = _lInstData.RunQuery(new InstrumentUtilizationAreasQuery()).ToList();
            InstrumentTypeList = _lInstData.RunQuery(new InstrumentTypesQuery()).ToList();
            ManufacturerList = _lInstData.RunQuery(new OrganizationsQuery() { Role = OrganizationsQuery.OrganizationRoles.Manufacturer })
                                                                        .ToList();
            CalibrationLabList = _lInstData.RunQuery(new OrganizationsQuery() { Role = OrganizationsQuery.OrganizationRoles.CalibrationLab })
                                                                        .ToList();

            PropertiesToAdd = new List<InstrumentProperty>();
            PropertiesToRemove = new List<InstrumentProperty>();
            PropertyList = new ObservableCollection<InstrumentProperty>();

            AddCalibrationCommand = new DelegateCommand(
                () =>
                {
                    if (_instrumentService.ShowNewCalibrationDialog(_instance) != null)
                        RefreshCalibrations();
                },
                () => IsInstrumentAdmin);

            AddFileCommand = new DelegateCommand(
                () =>
                {
                    OpenFileDialog fileDialog = new OpenFileDialog
                    {
                        Multiselect = true
                    };

                    if (fileDialog.ShowDialog() == DialogResult.OK)
                    {
                        IEnumerable<InstrumentFile> fileList = fileDialog.FileNames.Select(pt => new InstrumentFile() { Path = pt });

                        _lInstData.Execute(new BulkInsertEntitiesCommand<LInstContext>(fileList));

                        RaisePropertyChanged("FileList");
                    }
                });

            AddMaintenanceEventCommand = new DelegateCommand(
                () =>
                {
                    _instrumentService.ShowNewMaintenanceDialog(_instance);
                    RefreshMaintenanceEventList();
                },
                () => IsInstrumentAdmin);


            DeleteCalibrationCommand = new DelegateCommand(
                () =>
                {
                    DialogResult confirmation = MessageBox.Show("Il Report selezionato verrà eliminato, continuare?",
                                                            "Conferma Eliminazione",
                                                            MessageBoxButtons.YesNo);
                    if (confirmation == DialogResult.Yes)
                    {
                        _lInstData.Execute(new DeleteEntityCommand<LInstContext>(_selectedCalibration));
                        RefreshCalibrations();
                    }
                },
                () => Thread.CurrentPrincipal.IsInRole(UserRoleNames.InstrumentAdmin) && _selectedCalibration != null);

            OpenFileCommand = new DelegateCommand(
                () =>
                {
                    try
                    {
                        System.Diagnostics.Process.Start(_selectedFile.Path);
                    }
                    catch (Exception)
                    {
                        _eventAggregator.GetEvent<StatusNotificationIssued>().Publish("File non trovato");
                    }
                },
                () => _selectedFile != null);

            RemoveFileCommand = new DelegateCommand(
                () =>
                {
                    _lInstData.Execute(new DeleteEntityCommand<LInstContext>(_selectedFile));

                    RaisePropertyChanged("FileList");
                    SelectedFile = null;
                },
                () => _selectedFile != null);

            SaveCommand = new DelegateCommand(
                () =>
                {
                    _lInstData.Execute(new UpdateEntityCommand<LInstContext>(_instance));
                    _lInstData.Execute(new BulkInsertEntitiesCommand<LInstContext>(PropertiesToAdd));
                    _lInstData.Execute(new BulkDeleteEntitiesCommand<LInstContext>(PropertiesToRemove));
                    _lInstData.Execute(new BulkUpdateEntitiesCommand<LInstContext>(PropertyList));
                    ClearPropertyPendingUpdates();

                    EditMode = false;
                    _eventAggregator.GetEvent<InstrumentListUpdateRequested>()
                                    .Publish();
                },
                () => _editMode);

            StartEditCommand = new DelegateCommand(
                () =>
                {
                    EditMode = true;
                },
                () => IsInstrumentAdmin && !_editMode);

            #region Event Subscriptions

            _eventAggregator.GetEvent<CalibrationIssued>()
                            .Subscribe(
                            report =>
                            {
                                if (report.InstrumentID == _instance?.ID)
                                    RaisePropertyChanged("CalibrationReportList");
                            });

            #endregion Event Subscriptions
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Clears the lists for pending Insert/Remove operation on InstrumentProperties
        /// </summary>
        private void ClearPropertyPendingUpdates()
        {
            PropertiesToAdd.Clear();
            PropertiesToRemove.Clear();
        }

        private void OnPropertiesListChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
                foreach (InstrumentProperty ip in e.NewItems.OfType<InstrumentProperty>())
                {
                    ip.InstrumentID = _instance.ID;
                    PropertiesToAdd.Add(ip);
                }

            if (e.OldItems != null)
                PropertiesToRemove.AddRange(e.OldItems.OfType<InstrumentProperty>());
        }

        /// <summary>
        /// Queries the database for the list of CalibrationReport entries associated with the current Instrument instance 
        /// and sets the value of CalibrationReportList, then raises RaisePropertyChanged
        /// </summary>
        private void RefreshCalibrations()
        {
            SelectedCalibration = null;

            if (_instance == null)
                CalibrationReportList = new List<CalibrationReport>();

            CalibrationReportList = _lInstData.RunQuery(new CalibrationReportsQuery())
                            .Where(crep => crep.InstrumentID == _instance.ID).ToList();

            RaisePropertyChanged("CalibrationReportList");
        }

        private void RefreshMaintenanceEventList()
        {
            MaintenanceEventList = _lInstData.RunQuery(new MaintenanceEventsQuery() { InstrumentID = _instance?.ID }).ToList();
            RaisePropertyChanged("MaintenanceEventList");
        }

        private void RefreshPropertyList()
        {
            PropertyList = (_instance == null) ? new ObservableCollection<InstrumentProperty>() : new ObservableCollection<InstrumentProperty>(_lInstData.RunQuery(new InstrumentPropertiesQuery(_instance.ID)));

            PropertyList.CollectionChanged += OnPropertiesListChanged;

            ClearPropertyPendingUpdates();
            RaisePropertyChanged("PropertyList");
        }

        /// <summary>
        /// Calculates the new CalibrationDueDate using the provided parameters and sets the value in the current instance
        /// Finally Calls RaisePropertyChanged on CalibrationDueDate
        /// </summary>
        private void UpdateCalibrationDueDate()
        {
            if (!_instance.IsUnderControl)
                _instance.CalibrationDueDate = null;
            else
            {
                DateTime? lastCalibration = LastCalibrationDate;
                if (lastCalibration != null)
                    _instance.CalibrationDueDate = lastCalibration.Value.AddMonths((int)CalibrationInterval);
                else
                    _instance.CalibrationDueDate = DateTime.Today;
            }

            RaisePropertyChanged("CalibrationDueDate");
        }

        #endregion Methods

        #region Properties

        public DelegateCommand AddCalibrationCommand { get; }

        public DelegateCommand AddFileCommand { get; }

        public DelegateCommand AddMaintenanceEventCommand { get; }
        public DelegateCommand DeleteCalibrationCommand { get; }
        public DelegateCommand AddMethodAssociationCommand { get; }

        public DelegateCommand AddPropertyCommand { get; }

        public IEnumerable<InstrumentUtilizationArea> AreaList { get; }

        public DateTime? CalibrationDueDate => _instance?.CalibrationDueDate;

        public int? CalibrationInterval
        {
            get
            {
                if (_instance == null)
                    return 0;

                return _instance.CalibrationInterval;
            }
            set
            {
                _instance.CalibrationInterval = value;
                UpdateCalibrationDueDate();
            }
        }

        public IEnumerable<Organization> CalibrationLabList { get; }

        public IEnumerable<CalibrationReport> CalibrationReportList { get; private set; }

        public bool CanEditCalibrationParam => EditMode && IsUnderControl;

        public bool CanModify => !_editMode;

        public bool CanModifyInstrumentInfo => true;

        public bool EditMode
        {
            get { return _editMode; }
            set
            {
                _editMode = value;
                RaisePropertyChanged();
                RaisePropertyChanged("CanModify");

                SaveCommand.RaiseCanExecuteChanged();
                StartEditCommand.RaiseCanExecuteChanged();
            }
        }

        public IEnumerable<InstrumentMaintenanceEvent> EventList => (_instance == null) ? new List<InstrumentMaintenanceEvent>()
                                                                                        : _lInstData.RunQuery(new MaintenanceEventsQuery() { InstrumentID = _instance.ID }).ToList();

        public string InstrumentCode
        {
            get
            {
                if (_instance == null)
                    return null;
                return _instance.Code;
            }

            set
            {
                if (_instance == null)
                    return;

                _instance.Code = value;
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

        public string InstrumentEditCalibrationEditRegionName => RegionNames.InstrumentEditCalibrationEditRegion;

        public Instrument InstrumentInstance
        {
            get { return _instance; }
            set
            {
                _instance = value;
                _lInstData.Execute(new ReloadEntityCommand<LInstContext>(_instance));

                _selectedArea = AreaList.FirstOrDefault(iua => iua.ID == _instance.UtilizationAreaID);

                EditMode = false;
                SelectedCalibration = null;
                SelectedEvent = null;
                SelectedFile = null;

                RefreshPropertyList();
                RefreshMaintenanceEventList();
                RefreshCalibrations();

                RaisePropertyChanged("CalibrationDueDate");
                RaisePropertyChanged("CalibrationInterval");
                RaisePropertyChanged("InstrumentCode");
                RaisePropertyChanged("InstrumentDescription");
                RaisePropertyChanged("InstrumentManufacturer");
                RaisePropertyChanged("InstrumentModel");
                RaisePropertyChanged("InstrumentSerialNumber");
                RaisePropertyChanged("InstrumentType");
                RaisePropertyChanged("IsInService");
                RaisePropertyChanged("IsUnderControl");
                RaisePropertyChanged("LastCalibrationDate");
                RaisePropertyChanged("SelectedCalibrationLab");
                RaisePropertyChanged("SelectedArea");
            }
        }

        public Organization InstrumentManufacturer
        {
            get
            {
                if (_instance == null)
                    return null;
                else
                    return ManufacturerList.FirstOrDefault(manuf => manuf.ID == _instance.ManufacturerID);
            }

            set
            {
                if (_instance == null)
                    return;
                else
                    _instance.ManufacturerID = value.ID;
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

            set
            {
                if (_instance == null)
                    return;

                _instance.Model = value;
            }
        }

        public string InstrumentSerialNumber
        {
            get => _instance?.SerialNumber;

            set
            {
                if (_instance == null)
                    return;

                _instance.SerialNumber = value;
            }
        }

        public InstrumentType InstrumentType
        {
            get
            {
                if (_instance == null)
                    return null;
                else
                    return InstrumentTypeList.First(itt => itt.ID == _instance.InstrumentTypeID);
            }

            set
            {
                if (_instance == null)
                    return;
                else
                    _instance.InstrumentTypeID = value.ID;
            }
        }

        public IEnumerable<InstrumentType> InstrumentTypeList { get; }

        public bool IsInService
        {
            get
            {
                if (_instance == null)
                    return false;

                return _instance.IsInService;
            }

            set
            {
                _instance.IsInService = value;
                if (!value)
                    IsUnderControl = false;
            }
        }

        public bool IsUnderControl
        {
            get
            {
                return _instance == null ? false : _instance.IsUnderControl;
            }
            set
            {
                _instance.IsUnderControl = value;
                UpdateCalibrationDueDate();
                RaisePropertyChanged("IsUnderControl");
            }
        }

        public DateTime? LastCalibrationDate => (_instance == null) ? null : _lInstData.RunQuery(new LastCalibrationDateQuery(_instance.ID));

        public IEnumerable<InstrumentMaintenanceEvent> MaintenanceEventList { get; set; }

        public IEnumerable<Organization> ManufacturerList { get; }

        public DelegateCommand OpenFileCommand { get; }

        public ObservableCollection<InstrumentProperty> PropertyList { get; set; }

        public DelegateCommand RemoveFileCommand { get; }
        public DelegateCommand SaveCommand { get; }

        public InstrumentUtilizationArea SelectedArea
        {
            get { return _selectedArea; }
            set
            {
                _selectedArea = value;
                if (_instance != null)
                    _instance.UtilizationAreaID = value.ID;
            }
        }

        public CalibrationReport SelectedCalibration
        {
            get { return _selectedCalibration; }
            set
            {
                _selectedCalibration = value;
                DeleteCalibrationCommand.RaiseCanExecuteChanged();

                RaisePropertyChanged("SelectedCalibration");

                NavigationToken token = new NavigationToken(InstrumentViewNames.CalibrationReportEditView,
                                                            _selectedCalibration,
                                                            RegionNames.InstrumentEditCalibrationEditRegion);

                _eventAggregator.GetEvent<NavigationRequested>()
                                .Publish(token);
            }
        }

        public Organization SelectedCalibrationLab
        {
            get
            {
                if (_instance == null)
                    return null;
                else
                    return CalibrationLabList.FirstOrDefault(clab => clab.ID == _instance.CalibrationResponsibleID);
            }

            set
            {
                if (_instance == null)
                    return;
                else
                    _instance.CalibrationResponsibleID = value.ID;
            }
        }

        public InstrumentMaintenanceEvent SelectedEvent
        {
            get { return _selectedEvent; }
            set
            {
                _selectedEvent = value;
                RaisePropertyChanged();
            }
        }

        public InstrumentFile SelectedFile
        {
            get { return _selectedFile; }
            set
            {
                _selectedFile = value;
                RaisePropertyChanged("SelectedFile");

                OpenFileCommand.RaiseCanExecuteChanged();
                RemoveFileCommand.RaiseCanExecuteChanged();
            }
        }

        public DelegateCommand StartEditCommand { get; }
        private bool IsInstrumentAdmin => Thread.CurrentPrincipal.IsInRole(UserRoleNames.InstrumentAdmin);
        private List<InstrumentProperty> PropertiesToAdd { get; set; }

        private List<InstrumentProperty> PropertiesToRemove { get; set; }

        #endregion Properties
    }
}