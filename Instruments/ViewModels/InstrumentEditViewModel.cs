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
        private DelegateCommand _addCalibration, _addMaintenanceEvent, _addMethodAssociation,
                                _removeMethodAssociation, _setModify;
        private EventAggregator _eventAggregator;
        private IEnumerable<InstrumentType> _instrumentTypeList;
        private IEnumerable<Organization> _manufacturerList;
        private Instrument _instance;
        private Method _selectedAssociated, _selectedUnassociated;
        private Property _filterProperty;

        public InstrumentEditViewModel(DBPrincipal principal,
                                        EventAggregator aggregator) : base()
        {
            _editMode = false;
            _eventAggregator = aggregator;
            _principal = principal;
            _instrumentTypeList = DataService.GetInstrumentTypes();
            _manufacturerList = OrganizationService.GetOrganizations(OrganizationRoleNames.Manufacturer);

            _eventAggregator.GetEvent<CommitRequested>()
                            .Subscribe(() =>
                            {
                                if (_editMode)
                                {
                                    _instance.Update();
                                    EditMode = false;
                                }
                            });

            _addCalibration = new DelegateCommand(
                () =>
                {
                    _eventAggregator.GetEvent<NewCalibrationRequested>()
                                    .Publish(_instance);
                });

            _addMaintenanceEvent = new DelegateCommand(
                () =>
                {
                    _eventAggregator.GetEvent<NewMaintenanceEventRequested>()
                                    .Publish(_instance);
                });

            _addMethodAssociation = new DelegateCommand(
                () =>
                {
                    _instance.AddMethodAssociation(_selectedUnassociated);
                    SelectedUnassociatedMethod = null;
                    RaisePropertyChanged("AssociatedMethods");
                    RaisePropertyChanged("UnassociatedMethods");
                },
                () => _selectedUnassociated != null);

            _removeMethodAssociation = new DelegateCommand(
                () =>
                {
                    _instance.RemoveMethodAssociation(_selectedAssociated);
                    SelectedAssociatedMethod = null;
                    RaisePropertyChanged("AssociatedMethods");
                    RaisePropertyChanged("UnassociatedMethods");
                },
                () => _selectedAssociated != null);

            _setModify = new DelegateCommand(
                () =>
                {
                    EditMode = true;
                });
        }

        public DelegateCommand AddCalibrationCommand
        {
            get { return _addCalibration; }
        }

        public DelegateCommand AddMaintenanceEvent
        {
            get { return _addMaintenanceEvent; }
        }
        
        public DelegateCommand AddMethodAssociationCommand
        {
            get { return _addMethodAssociation; }
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
                    
                return _instance.CalibrationReports; 
            }
        }

        public Visibility CalibrationTabVisible
        {
            get
            {
                if (_instance == null)
                    return Visibility.Visible;

                else if (_instance.IsUnderControl)
                    return Visibility.Visible;

                else
                    return Visibility.Hidden;
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
            }
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

        public Instrument InstrumentInstance
        {
            get { return _instance; }
            set
            {
                _instance = value;
                _instance.Load();

                EditMode = false;
                SelectedAssociatedMethod = null;
                SelectedCalibration = null;
                SelectedUnassociatedMethod = null;

                RaisePropertyChanged("AssociatedMethods");
                RaisePropertyChanged("CalibrationReportList");
                RaisePropertyChanged("CalibrationTabVisible");
                RaisePropertyChanged("InstrumentCode");
                RaisePropertyChanged("InstrumentDescription");
                RaisePropertyChanged("InstrumentManufacturer");
                RaisePropertyChanged("InstrumentModel");
                RaisePropertyChanged("InstrumentSerialNumber");
                RaisePropertyChanged("InstrumentType");
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
            }
        }

        public DelegateCommand SetModifyCommand
        {
            get { return _setModify; }
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
