using DBManager;
using DBManager.EntityExtensions;
using DBManager.Services;
using Infrastructure;
using Infrastructure.Events;
using Infrastructure.Wrappers;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace Instruments.ViewModels
{
    public class InstrumentEditViewModel : BindableBase
    {
        private bool _editMode;
        private CalibrationReport _selectedCalibration;
        private DBPrincipal _principal;
        private DelegateCommand _addCalibration,
                                _addFileCommand,
                                _addMaintenanceEvent, 
                                _addMethodAssociation,
                                _addProperty,
                                _openFileCommand,
                                _removeFileCommand,
                                _removeMethodAssociation, 
                                _save,
                                _startEdit;
        private EventAggregator _eventAggregator;
        private IEnumerable<InstrumentMeasurablePropertyWrapper> _propertyList;
        private IEnumerable<InstrumentType> _instrumentTypeList;
        private IEnumerable<InstrumentUtilizationArea> _areaList;
        private IEnumerable<Organization> _manufacturerList,
                                        _calibrationLabList;
        private Instrument _instance;
        private InstrumentMeasurablePropertyWrapper _selectedMeasurableProperty;
        private InstrumentUtilizationArea _selectedArea;
        private Method _selectedAssociated, _selectedUnassociated;
        private Property _filterProperty;

        public InstrumentEditViewModel(DBPrincipal principal,
                                        EventAggregator aggregator) : base()
        {
            _editMode = false;
            _eventAggregator = aggregator;
            _principal = principal;
            _areaList = InstrumentService.GetUtilizationAreas();
            _instrumentTypeList = InstrumentService.GetInstrumentTypes();
            _manufacturerList = OrganizationService.GetOrganizations(OrganizationRoleNames.Manufacturer);
            _calibrationLabList = OrganizationService.GetOrganizations(OrganizationRoleNames.CalibrationLab);
            
            _addCalibration = new DelegateCommand(
                () =>
                {
                    _eventAggregator.GetEvent<NewCalibrationRequested>()
                                    .Publish(_instance);
                },
                () => IsInstrumentAdmin);

            _addFileCommand = new DelegateCommand(
                () =>
                {

                    OpenFileDialog fileDialog = new OpenFileDialog();
                    fileDialog.Multiselect = true;

                    if (fileDialog.ShowDialog() == DialogResult.OK)
                    {

                        IEnumerable<InstrumentFiles> fileList = fileDialog.FileNames
                                                                            .Select(file => new InstrumentFiles()
                                                                            {
                                                                                InstrumentID = _instance.ID,
                                                                                Path = file,
                                                                                Description = ""
                                                                            });

                        InstrumentService.AddInstrumentFiles(fileList);
                        RaisePropertyChanged("FileList");
                    }
                });

            _addMaintenanceEvent = new DelegateCommand(
                () =>
                {
                    _eventAggregator.GetEvent<MaintenanceEventCreationRequested>()
                                    .Publish(_instance);
                },
                () => IsInstrumentAdmin);

            _addMethodAssociation = new DelegateCommand(
                () =>
                {
                    _instance.AddMethodAssociation(_selectedUnassociated);
                    SelectedUnassociatedMethod = null;
                    RaisePropertyChanged("AssociatedMethods");
                    RaisePropertyChanged("UnassociatedMethods");
                },
                () => IsInstrumentAdmin && _selectedUnassociated != null);

            _addProperty = new DelegateCommand(
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

            _removeMethodAssociation = new DelegateCommand(
                () =>
                {
                    _instance.RemoveMethodAssociation(_selectedAssociated);
                    SelectedAssociatedMethod = null;
                    RaisePropertyChanged("AssociatedMethods");
                    RaisePropertyChanged("UnassociatedMethods");
                },
                () => IsInstrumentAdmin && _selectedAssociated != null);

            _save = new DelegateCommand(
                () =>
                {
                    _instance.Update();
                    foreach (InstrumentMeasurablePropertyWrapper impw in _propertyList.Where(imp => imp.IsModified))
                        impw.PropertyInstance.Update();

                    EditMode = false;
                    _eventAggregator.GetEvent<InstrumentListUpdateRequested>()
                                    .Publish();
                },
                () => _editMode);

            _startEdit = new DelegateCommand(
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

            #endregion


        }

        public DelegateCommand AddCalibrationCommand
        {
            get { return _addCalibration; }
        }

        public DelegateCommand AddMaintenanceEventCommand
        {
            get { return _addMaintenanceEvent; }
        }
        
        public DelegateCommand AddMethodAssociationCommand
        {
            get { return _addMethodAssociation; }
        }

        public DelegateCommand AddPropertyCommand
        {
            get { return _addProperty; }
        }

        public IEnumerable<InstrumentUtilizationArea> AreaList
        {
            get { return _areaList; }
        }

        public IEnumerable<Method> AssociatedMethods
        {
            get { return _instance.GetAssociatedMethods(); }
        }

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

        public IEnumerable<Organization> CalibrationLabList
        {
            get { return _calibrationLabList; }
        }

        public IEnumerable<CalibrationReport> CalibrationReportList
        {
            get 
            { 
                if (_instance == null)
                    return new List<CalibrationReport>();
                    
                return _instance.GetCalibrationReports(); 
            }
        }


        public bool CanEditCalibrationParam
        {
            get
            {
                return EditMode && IsUnderControl;
            }
        }

        public bool CanModify
        {
            get { return !_editMode; }
        }

        public bool CanModifyInstrumentInfo
        {
            get { return true; }
        }

        public bool EditMode
        {
            get { return _editMode; }
            set
            {
                _editMode = value;
                RaisePropertyChanged("EditMode");
                RaisePropertyChanged("CanModify");

                _save.RaiseCanExecuteChanged();
                _startEdit.RaiseCanExecuteChanged();
            }
        }

        public IEnumerable<InstrumentMaintenanceEvent> EventList
        {
            get { return _instance.GetMaintenanceEvents(); }
        }

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

        public string InstrumentEditCalibrationEditRegionName
        {
            get { return RegionNames.InstrumentEditCalibrationEditRegion; }
        }

        public string InstrumentEditMetrologyRegionName
        {
            get { return RegionNames.InstrumentEditMetrologyRegion; }
        }

        public Instrument InstrumentInstance
        {
            get { return _instance; }
            set
            {
                _instance = value;
                _instance.Load();

                _selectedArea = _areaList.FirstOrDefault(iua => iua.ID == _instance.UtilizationAreaID);
                _propertyList = _instance.GetMeasurableProperties()
                                        .Select(imp => new InstrumentMeasurablePropertyWrapper(imp))
                                        .ToList();

                EditMode = false;
                SelectedAssociatedMethod = null;
                SelectedCalibration = null;
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
                    return _manufacturerList.FirstOrDefault(manuf => manuf.ID == _instance.manufacturerID);
            }

            set
            {
                if (_instance == null)
                    return;

                else
                    _instance.manufacturerID = value.ID;
            }
        }

        public IEnumerable<InstrumentMeasurablePropertyWrapper> InstrumentMeasurablePropertyList
        {
            get { return _propertyList; }
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
                    return _instrumentTypeList.First(itt => itt.ID == _instance.InstrumentTypeID);
            }

            set
            {
                if (_instance == null)
                    return;
                else
                    _instance.InstrumentTypeID = value.ID;
            }
        }

        public IEnumerable<InstrumentType> InstrumentTypeList
        {
            get { return _instrumentTypeList; }
        }

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

        private bool IsInstrumentAdmin
        {
            get { return _principal.IsInRole(UserRoleNames.InstrumentAdmin); }
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

        public IEnumerable<InstrumentMaintenanceEvent> MaintenanceEventList
        {
            get { return _instance.GetMaintenanceEvents(); }
        }

        public IEnumerable<Organization> ManufacturerList
        {
            get { return _manufacturerList; }
        }

        public IEnumerable<Property> PropertyList
        {
            get { return DataService.GetProperties(); }
        }

        public DelegateCommand RemoveMethodAssociationCommand
        {
            get { return _removeMethodAssociation; }
        }

        public DelegateCommand SaveCommand
        {
            get { return _save; }
        }

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
                _removeMethodAssociation.RaiseCanExecuteChanged();
            }
        }

        public InstrumentMeasurablePropertyWrapper SelectedInstrumentMeasurableProperty
        {
            get { return _selectedMeasurableProperty; }
            set
            {
                _selectedMeasurableProperty = value;
            }
        }

        public Method SelectedUnassociatedMethod
        {
            get { return _selectedUnassociated; }
            set
            {
                _selectedUnassociated = value;
                RaisePropertyChanged("SelectedUnassociatedMethod");
                _addMethodAssociation.RaiseCanExecuteChanged();
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
                    return _calibrationLabList.FirstOrDefault(clab => clab.ID == _instance.CalibrationResponsibleID);
            }

            set
            {
                if (_instance == null)
                    return;

                else
                    _instance.CalibrationResponsibleID = value.ID;
            }
        }

        public DelegateCommand StartEditCommand
        {
            get { return _startEdit; }
        }

        public IEnumerable<Method> UnassociatedMethods
        {
            get
            {
                return _instance.GetUnassociatedMethods();
            }
        }
    }
}
