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
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace Instruments.ViewModels
{
    public class NewCalibrationDialogViewModel : BindableBase
    {
        private bool _isVerificationOnly;
        private CalibrationReport _reportInstance;
        private DateTime _calibrationDate;
        private DBEntities _entities;
        private DBPrincipal _principal;
        private DelegateCommand _removeReference;
        private DelegateCommand<string> _addReference;
        private DelegateCommand<Window> _cancel, _confirm;
        private EventAggregator _eventAggregator;
        private IDataService _dataService;
        private IEnumerable<Organization> _labList;
        private Instrument _instumentInstance, _selectedReference;
        private Person _selectedTech;
        private string _calibrationNotes, _calibrationResult, _referenceCode;
        private ObservableCollection<Instrument> _referenceList;
        private Organization _selectedLab;

        public NewCalibrationDialogViewModel(DBEntities entities,
                                            DBPrincipal principal,
                                            EventAggregator eventAggregator,
                                            IDataService dataService) : base()
        {
            _dataService = dataService;
            _entities = entities;
            _isVerificationOnly = false;
            _referenceList = new ObservableCollection<Instrument>();
            _labList = _dataService.GetOrganizations(OrganizationRoleNames.CalibrationLab);
            _eventAggregator = eventAggregator;
            _principal = principal;

            _calibrationDate = DateTime.Now.Date;
            
            _addReference = new DelegateCommand<string>(
                code =>
                {
                    Instrument tempRef = _entities.Instruments.FirstOrDefault(inst => inst.Code == code);
                    if (tempRef != null)
                    {
                        _referenceList.Add(tempRef);
                        ReferenceCode = "";
                    }
                });

            _cancel = new DelegateCommand<Window>(
                parentDialog =>
                {
                    parentDialog.DialogResult = false;
                });

            _confirm = new DelegateCommand<Window>(
                parentDialog =>
                {
                    _reportInstance = new CalibrationReport();
                    _reportInstance.Date = _calibrationDate;
                    _reportInstance.Year = DateTime.Now.Year - 2000;
                    _reportInstance.Number = InstrumentService.GetNextCalibrationNumber(_reportInstance.Year);
                    _reportInstance.Instrument = _instumentInstance;
                    _reportInstance.IsVerification = _isVerificationOnly;
                    _reportInstance.laboratoryID = _selectedLab.ID;
                    _reportInstance.Notes = "";
                    _reportInstance.ResultID = 1;

                    if (IsNotExternalLab)
                    {
                        _reportInstance.Tech = _selectedTech;

                        foreach (Instrument refInstrument in _referenceList)
                            _reportInstance.ReferenceInstruments.Add(refInstrument);
                    }
                    
                    foreach (InstrumentMeasurableProperty imp in _instumentInstance.GetMeasurableProperties())
                    {
                        CalibrationReportInstrumentPropertyMapping cripm = new CalibrationReportInstrumentPropertyMapping()
                        {
                            ExtendedUncertainty = 0,
                            LowerRangeValue = imp.RangeLowerLimit,
                            MeasurablePropertyID = imp.ID,
                            MeasurementUnitID = imp.UnitID,
                            UpperRangeValue = imp.RangeUpperLimit
                        };

                        _reportInstance.InstrumentMeasurablePropertyMappings.Add(cripm);
                    }

                    _entities.CalibrationReports.Add(_reportInstance);
                    _entities.SaveChanges();

                    parentDialog.DialogResult = true;
                });
            
            _removeReference = new DelegateCommand(
                () =>
                {
                    _referenceList.Remove(_selectedReference);
                    SelectedReference = null;
                },
                () => _selectedReference != null);
        }
        
        public DelegateCommand<string> AddReferenceCommand
        {
            get { return _addReference; }
        }

        public DateTime CalibrationDate
        {
            get { return _calibrationDate; }
            set
            {
                _calibrationDate = value;
            }
        }

        public string CalibrationNotes
        {
            get { return _calibrationNotes; }
            set 
            {
                _calibrationNotes = value;
            }
        }

        public string CalibrationResult
        {
            get { return _calibrationResult; }
            set 
            {
                _calibrationResult = value;
            }
        }

        public DelegateCommand<Window> CancelCommand
        {
            get { return _cancel; }
        }

        public DelegateCommand<Window> ConfirmCommand
        {
            get { return _confirm; }
        }

        public string FileListRegionName
        {
            get { return RegionNames.NewCalibrationFileListRegion; }
        }
        
        public string InstrumentCode
        {
            get
            {
                if (_instumentInstance == null)
                    return null;

                return _instumentInstance.Code;
            }
        }

        public Instrument InstrumentInstance
        {
            get { return _instumentInstance; }
            set
            {
                _instumentInstance = _entities.Instruments.First(ins => ins.ID == value.ID);
                SelectedLab = _labList.FirstOrDefault(lab => lab.ID == _instumentInstance.CalibrationResponsibleID);
                RaisePropertyChanged("InstrumentCode");
                RaisePropertyChanged("PropertyList");
                RaisePropertyChanged("SelectedLab");
            }
        }

        public bool IsVerificationOnly
        {
            get { return _isVerificationOnly; }
            set { _isVerificationOnly = value; }
        }
            

        public bool IsNotExternalLab
        {
            get
            {
                if (_selectedLab == null)
                    return false;

                return _selectedLab.Name == "Vulcaflex";
            }
        }

        public IEnumerable<Organization> LabList
        {
            get
            {
                return _labList;
            }
        }

        public string ReferenceCode
        {
            get { return _referenceCode; }
            set
            {
                _referenceCode = value;
                RaisePropertyChanged();
            }
        }

        public ObservableCollection<Instrument> ReferenceList
        {
            get { return _referenceList; }
        }

        public DelegateCommand RemoveReference
        {
            get { return _removeReference; }
        }

        public CalibrationReport ReportInstance
        {
            get { return _reportInstance; }
        }
        
        public Organization SelectedLab
        {
            get { return _selectedLab; }
            set
            {
                _selectedLab = value;
                RaisePropertyChanged("SelectedLab");
                RaisePropertyChanged("IsNotExternalLab");
            }
        }

        public Instrument SelectedReference
        {
            get
            {
                return _selectedReference;
            }

            set
            {
                _selectedReference = value;
                _removeReference.RaiseCanExecuteChanged();
                RaisePropertyChanged();
            }
        }

        public Person SelectedTech
        {
            get { return _selectedTech; }
            set
            {
                _selectedTech = value;
            }
        }

        public List<Person> TechList
        {
            get
            {
                PersonRole tempRole = _entities.PersonRoles.First(prr => prr.Name == PersonRoleNames.CalibrationTech);
                return new List<Person>(tempRole.RoleMappings.Where(prm => prm.IsSelected)
                                            .Select(prm => prm.Person));
            }
        }
    }
}
