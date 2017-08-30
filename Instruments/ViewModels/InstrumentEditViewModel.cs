using DBManager;
using DBManager.EntityExtensions;
using DBManager.Services;
using Infrastructure;
using Infrastructure.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Instruments.ViewModels
{
    public class InstrumentEditViewModel : BindableBase
    {
        private bool _editMode;
        private CalibrationReport _selectedCalibration;
        private DBPrincipal _principal;
        private DelegateCommand _addCalibration, 
                                _addMaintenanceEvent, 
                                _addMethodAssociation,
                                _addProperty,
                                _removeMethodAssociation, 
                                _save,
                                _startEdit;
        private EventAggregator _eventAggregator;
        private IEnumerable<InstrumentType> _instrumentTypeList;
        private IEnumerable<InstrumentUtilizationArea> _areaList;
        private IEnumerable<Organization> _manufacturerList;
        private Instrument _instance;
        private InstrumentMeasurableProperty _selectedMeasurableProperty;
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
            
            _addCalibration = new DelegateCommand(
                () =>
                {
                    _eventAggregator.GetEvent<NewCalibrationRequested>()
                                    .Publish(_instance);
                },
                () => IsInstrumentAdmin);

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
                            ControlPeriod = 0,
                            Description = "",
                            InstrumentID = _instance.ID,
                            IsUnderControl = false,
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
                    EditMode = false;
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

        public IEnumerable<InstrumentUtilizationArea> AreaList
        {
            get { return _areaList; }
        }

        public IEnumerable<Method> AssociatedMethods
        {
            get { return _instance.GetAssociatedMethods(); }
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

        public string InstrumentEditMeasurablePropertyEditRegionName
        {
            get { return RegionNames.InstrumentEditMeasurablePropertyEditRegion; }
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

                EditMode = false;
                SelectedAssociatedMethod = null;
                SelectedCalibration = null;
                SelectedInstrumentMeasurableProperty = null;
                SelectedUnassociatedMethod = null;

                RaisePropertyChanged("AssociatedMethods");
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
                    return _manufacturerList.First(manuf => manuf.ID == _instance.Manufacturer.ID);
            }

            set
            {
                if (_instance == null)
                    return;

                else
                    _instance.Manufacturer = value;
            }
        }

        public IEnumerable<InstrumentMeasurableProperty> InstrumentMeasurablePropertyList
        {
            get { return _instance.GetMeasurableProperties(); }
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
                    return _instrumentTypeList.First(itt => itt.ID == _instance.InstrumentType.ID);
            }

            set
            {
                if (_instance == null)
                    return;
                else
                    _instance.InstrumentType = value;
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
                    foreach (InstrumentMeasurableProperty imp in InstrumentMeasurablePropertyList)
                    {
                        imp.IsUnderControl = false;
                        imp.UpdateCalibrationDueDate();
                    }
            }
        }

        private bool IsInstrumentAdmin
        {
            get { return _principal.IsInRole(UserRoleNames.InstrumentAdmin); }
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

        public InstrumentMeasurableProperty SelectedInstrumentMeasurableProperty
        {
            get { return _selectedMeasurableProperty; }
            set
            {
                _selectedMeasurableProperty = value;

                NavigationToken token = new NavigationToken(InstrumentViewNames.InstrumentMeasurablePropertyEditView,
                                                            _selectedMeasurableProperty,
                                                            RegionNames.InstrumentEditMeasurablePropertyEditRegion);

                _eventAggregator.GetEvent<NavigationRequested>()
                                .Publish(token);
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
