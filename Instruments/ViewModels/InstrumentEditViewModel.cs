using Controls.Views;
using DataAccess;
using Infrastructure;
using Infrastructure.Commands;
using Infrastructure.Events;
using Infrastructure.Queries;
using Infrastructure.Wrappers;
using LabDbContext;
using LabDbContext.EntityExtensions;
using LabDbContext.Services;
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
        private Property _filterProperty;
        private Instrument _instance;
        private InstrumentService _instrumentService;
        private IDataService<LabDbEntities> _labDbData;
        private InstrumentUtilizationArea _selectedArea;
        private Method _selectedAssociated, _selectedUnassociated;
        private CalibrationReport _selectedCalibration;
        private InstrumentMaintenanceEvent _selectedEvent;
        private InstrumentFiles _selectedFile;

        #endregion Fields

        #region Constructors

        public InstrumentEditViewModel(IDataService<LabDbEntities> labDbdata,
                                        IEventAggregator aggregator,
                                        InstrumentService instrumentService) : base()
        {
            _labDbData = labDbdata;
            _editMode = false;
            _eventAggregator = aggregator;
            _instrumentService = instrumentService;

            AreaList = _labDbData.RunQuery(new InstrumentUtilizationAreasQuery()).ToList();
            InstrumentTypeList = _labDbData.RunQuery(new InstrumentTypesQuery()).ToList();
            ManufacturerList = _labDbData.RunQuery(new OrganizationsQuery() { Role = OrganizationsQuery.OrganizationRoles.Manufacturer })
                                                                        .ToList();
            CalibrationLabList = _labDbData.RunQuery(new OrganizationsQuery() { Role = OrganizationsQuery.OrganizationRoles.CalibrationLab })
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
                        IEnumerable<string> fileList = fileDialog.FileNames;

                        _instance.AddFiles(fileList);
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

            AddMethodAssociationCommand = new DelegateCommand(
                () =>
                {
                    _instance.AddMethodAssociation(_selectedUnassociated);
                    SelectedUnassociatedMethod = null;
                    RaisePropertyChanged("AssociatedMethods");
                    RaisePropertyChanged("UnassociatedMethods");
                },
                () => IsInstrumentAdmin && _selectedUnassociated != null);

            AddPropertyCommand = new DelegateCommand(
                () =>
                {
                    Views.AddPropertyDialog propertyDialog = new Views.AddPropertyDialog();
                    if (propertyDialog.ShowDialog() == true)
                    {
                        InstrumentMeasurableProperty newIMP = new InstrumentMeasurableProperty()
                        {
                            CalibrationRangeLowerLimit = 0,
                            CalibrationRangeUpperLimit = 0,
                            Description = "",
                            InstrumentID = _instance.ID,
                            MeasurableQuantityID = propertyDialog.QuantityInstance.ID,
                            RangeLowerLimit = 0,
                            RangeUpperLimit = 0,
                            Resolution = 0,
                            TargetUncertainty = 0,
                            UnitID = propertyDialog.QuantityInstance.GetMeasurementUnits().First().ID
                        };

                        newIMP.Create();
                        RaisePropertyChanged("InstrumentMeasurablePropertyList");
                    }
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
                    _labDbData.Execute(new DeleteEntityCommand(_selectedFile));

                    RaisePropertyChanged("FileList");
                    SelectedFile = null;
                },
                () => _selectedFile != null);

            RemoveMethodAssociationCommand = new DelegateCommand(
                () =>
                {
                    _instance.RemoveMethodAssociation(_selectedAssociated);
                    SelectedAssociatedMethod = null;
                    RaisePropertyChanged("AssociatedMethods");
                    RaisePropertyChanged("UnassociatedMethods");
                },
                () => IsInstrumentAdmin && _selectedAssociated != null);

            SaveCommand = new DelegateCommand(
                () =>
                {
                    _instance.Update();
                    foreach (InstrumentMeasurablePropertyWrapper impw in InstrumentMeasurablePropertyList.Where(imp => imp.IsModified))
                        impw.PropertyInstance.Update();

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
                                if (report.instrumentID == _instance?.ID)
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

        public IEnumerable<Method> AssociatedMethods => _instance.GetAssociatedMethods();

        public string CalibrationDueDate
        {
            get
            {
                if (_instance == null || _instance.CalibrationDueDate == null)
                    return "//";

                return _instance?.CalibrationDueDate.Value.ToShortDateString();
            }
        }

        public int CalibrationInterval
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
                if (_instance.UpdateCalibrationDueDate())
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

                return _instance.GetCalibrationReports();
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

        public IEnumerable<InstrumentMaintenanceEvent> EventList => _instance.GetMaintenanceEvents();

        public Property FilterProperty
        {
            get { return _filterProperty; }
            set
            {
                _filterProperty = value;
                RaisePropertyChanged("FilterProperty");
            }
        }

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
                _instance.Load();

                _selectedArea = AreaList.FirstOrDefault(iua => iua.ID == _instance.UtilizationAreaID);
                InstrumentMeasurablePropertyList = _instance.GetMeasurableProperties()
                                        .Select(imp => new InstrumentMeasurablePropertyWrapper(imp))
                                        .ToList();

                EditMode = false;
                SelectedAssociatedMethod = null;
                SelectedCalibration = null;
                SelectedEvent = null;
                SelectedFile = null;
                SelectedInstrumentMeasurableProperty = null;
                SelectedUnassociatedMethod = null;

                RaisePropertyChanged("AssociatedMethods");
                RaisePropertyChanged("CalibrationDueDate");
                RaisePropertyChanged("CalibrationInterval");
                RaisePropertyChanged("CalibrationReportList");
                RaisePropertyChanged("CalibrationTabVisible");
                RaisePropertyChanged("EventList");
                RaisePropertyChanged("InstrumentCode");
                RaisePropertyChanged("InstrumentDescription");
                RaisePropertyChanged("InstrumentManufacturer");
                RaisePropertyChanged("InstrumentMeasurablePropertyList");
                RaisePropertyChanged("InstrumentModel");
                RaisePropertyChanged("InstrumentSerialNumber");
                RaisePropertyChanged("InstrumentType");
                RaisePropertyChanged("IsInService");
                RaisePropertyChanged("IsUnderControl");
                RaisePropertyChanged("LastCalibrationDate");
                RaisePropertyChanged("SelectedCalibrationLab");
                RaisePropertyChanged("SelectedArea");
                RaisePropertyChanged("UnassociatedMethods");
            }
        }

        public Organization InstrumentManufacturer
        {
            get
            {
                if (_instance == null)
                    return null;
                else
                    return ManufacturerList.FirstOrDefault(manuf => manuf.ID == _instance.manufacturerID);
            }

            set
            {
                if (_instance == null)
                    return;
                else
                    _instance.manufacturerID = value.ID;
            }
        }

        public IEnumerable<InstrumentMeasurablePropertyWrapper> InstrumentMeasurablePropertyList { get; private set; }

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
            get
            {
                if (_instance == null)
                    return null;
                else
                    return _instance.SerialNumber;
            }

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
                _instance.UpdateCalibrationDueDate();
                RaisePropertyChanged("CalibrationDueDate");
                RaisePropertyChanged("IsUnderControl");
            }
        }

        public string LastCalibrationDate
        {
            get
            {
                DateTime? lastCal = _instance?.GetLastCalibration()?.Date;

                return (lastCal != null) ? lastCal.Value.ToShortDateString() : "Mai";
            }
        }

        public IEnumerable<InstrumentMaintenanceEvent> MaintenanceEventList => _instance.GetMaintenanceEvents();

        public IEnumerable<Organization> ManufacturerList { get; }

        public DelegateCommand OpenFileCommand { get; }

        public IEnumerable<Property> PropertyList => _labDbData.RunQuery(new PropertiesQuery()).ToList();

        public DelegateCommand RemoveFileCommand { get; }

        public DelegateCommand RemoveMethodAssociationCommand { get; }

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

        public Method SelectedAssociatedMethod
        {
            get { return _selectedAssociated; }
            set
            {
                _selectedAssociated = value;
                RaisePropertyChanged("SelectedAssociatedMethod");
                RemoveMethodAssociationCommand.RaiseCanExecuteChanged();
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

        public InstrumentFiles SelectedFile
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

        public InstrumentMeasurablePropertyWrapper SelectedInstrumentMeasurableProperty { get; set; }

        public Method SelectedUnassociatedMethod
        {
            get { return _selectedUnassociated; }
            set
            {
                _selectedUnassociated = value;
                RaisePropertyChanged("SelectedUnassociatedMethod");
                AddMethodAssociationCommand.RaiseCanExecuteChanged();
            }
        }

        public DelegateCommand StartEditCommand { get; }

        public IEnumerable<Method> UnassociatedMethods => _instance.GetUnassociatedMethods();

        private bool IsInstrumentAdmin => Thread.CurrentPrincipal.IsInRole(UserRoleNames.InstrumentAdmin);

        #endregion Properties
    }
}