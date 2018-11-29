using Controls.Views;
using DataAccessCore;
using DataAccessCore.Commands;
using Infrastructure;
using Infrastructure.Commands;
using Infrastructure.Events;
using Infrastructure.Queries;
using Infrastructure.Wrappers;
using Instruments.Queries;
using LInst;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
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

            AddCalibrationCommand = new DelegateCommand(
                () =>
                {
                    _instrumentService.ShowNewCalibrationDialog(_instance);
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
                        IEnumerable<InstrumentFile> fileList = fileDialog.FileNames.Select(pt => new InstrumentFile() { Path = pt});

                        _lInstData.Execute(new BulkInsertEntitiesCommand<LInstContext>(fileList));

                        RaisePropertyChanged("FileList");
                    }
                });

            AddMaintenanceEventCommand = new DelegateCommand(
                () =>
                {
                    _instrumentService.ShowNewMaintenanceDialog(_instance);
                    RaisePropertyChanged("MaintenanceEventList");
                },
                () => IsInstrumentAdmin);

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

            _eventAggregator.GetEvent<MaintenanceEventCreated>()
                            .Subscribe(
                            maint =>
                            {
                                if (maint.InstrumentID == _instance?.ID)
                                    RaisePropertyChanged("MaintenanceEventList");
                            });

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

        #region Properties

        public DelegateCommand AddCalibrationCommand { get; }

        public DelegateCommand AddFileCommand { get; }

        public DelegateCommand AddMaintenanceEventCommand { get; }

        public DelegateCommand AddMethodAssociationCommand { get; }

        public DelegateCommand AddPropertyCommand { get; }

        public IEnumerable<InstrumentUtilizationArea> AreaList { get; }
        
        public string CalibrationDueDate
        {
            get
            {
                if (_instance == null || _instance.CalibrationDueDate == null)
                    return "//";

                return _instance?.CalibrationDueDate.Value.ToShortDateString();
            }
        }

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
                ///TODO
                    RaisePropertyChanged("CalibrationDueDate");
            }
        }

        public IEnumerable<Organization> CalibrationLabList { get; }

        public IEnumerable<CalibrationReport> CalibrationReportList
        {
            get
            {
                if (_instance == null)
                    return new List<CalibrationReport>();

                return _lInstData.RunQuery(new CalibrationReportsQuery())
                                .Where(crep => crep.InstrumentID == _instance.ID).ToList();
            }
        }

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

        public string InstrumentEditMetrologyRegionName => RegionNames.InstrumentEditMetrologyRegion;

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

                RaisePropertyChanged("CalibrationDueDate");
                RaisePropertyChanged("CalibrationInterval");
                RaisePropertyChanged("CalibrationReportList");
                RaisePropertyChanged("CalibrationTabVisible");
                RaisePropertyChanged("EventList");
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
                ///TODO 
                RaisePropertyChanged("CalibrationDueDate");
                RaisePropertyChanged("IsUnderControl");
            }
        }

        public string LastCalibrationDate
        {
            get
            {
                //TODO
                //DateTime? lastCal = _instance?.GetLastCalibration()?.Date;
                // (lastCal != null) ? lastCal.Value.ToShortDateString() :
                return "Mai";
            }
        }

        public IEnumerable<InstrumentMaintenanceEvent> MaintenanceEventList => _lInstData.RunQuery(new MaintenanceEventsQuery() {InstrumentID = _instance?.ID }).ToList();

        public IEnumerable<Organization> ManufacturerList { get; }

        public DelegateCommand OpenFileCommand { get; }

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

        #endregion Properties
    }
}