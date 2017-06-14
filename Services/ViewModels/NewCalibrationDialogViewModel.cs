using DBManager;
using Infrastructure;
using Infrastructure.Events;
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

namespace Services.ViewModels
{
    public class NewCalibrationDialogViewModel : BindableBase
    {
        private CalibrationFiles _selectedFile;
        private CalibrationReport _reportInstance;
        private DateTime _calibrationDate;
        private DBEntities _entities;
        private DBPrincipal _principal;
        private DelegateCommand _addFile, _openFile, _removeFile, _removeReference;
        private DelegateCommand<string> _addReference;
        private DelegateCommand<Window> _cancel, _confirm;
        private EventAggregator _eventAggregator;
        private Instrument _instumentInstance, _selectedReference;
        private Int32 _reportNumber;
        private Person _selectedTech;
        private string _calibrationNotes, _calibrationResult, _referenceCode;
        private ObservableCollection<CalibrationFiles> _calibrationFileList;
        private ObservableCollection<Instrument> _referenceList;
        private Organization _selectedLab;

        public NewCalibrationDialogViewModel(DBEntities entities,
                                            DBPrincipal principal,
                                            EventAggregator eventAggregator) : base()
        {
            _entities = entities;
            _calibrationFileList = new ObservableCollection<CalibrationFiles>();
            _referenceList = new ObservableCollection<Instrument>();
            _eventAggregator = eventAggregator;
            _principal = principal;

            _calibrationDate = DateTime.Now.Date;

            _addFile = new DelegateCommand(
                () =>
                {
                    OpenFileDialog fileDialog = new OpenFileDialog();
                    fileDialog.Multiselect = true;
                    
                    if (fileDialog.ShowDialog() == DialogResult.OK)
                    {
                        foreach (string pth in fileDialog.FileNames)
                        {
                            CalibrationFiles temp = new CalibrationFiles();
                            temp.Path = pth;
                            temp.Description = "";
                            _calibrationFileList.Add(temp);
                        }

                        RaisePropertyChanged("FileList");
                    }
                });

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
                    _reportInstance.Instrument = _instumentInstance;
                    _reportInstance.Laboratory = _selectedLab;
                    _reportInstance.Number = ReportNumber;
                    _reportInstance.Notes = "";
                    _reportInstance.Result = "";

                    if (IsNotExternalLab)
                    {
                        _reportInstance.Tech = _selectedTech;

                        foreach (Instrument refInstrument in _referenceList)
                            _reportInstance.ReferenceInstruments.Add(refInstrument);
                    }

                    foreach (CalibrationFiles calFile in _calibrationFileList)
                        _reportInstance.CalibrationFiles.Add(calFile);

                    _entities.CalibrationReports.Add(_reportInstance);
                    _entities.SaveChanges();

                    parentDialog.DialogResult = true;
                });

            _openFile = new DelegateCommand(
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

            _removeFile = new DelegateCommand(
                () =>
                {
                    _calibrationFileList.Remove(_selectedFile);
                    SelectedFile = null;
                },
                () => _selectedFile != null);

            _removeReference = new DelegateCommand(
                () =>
                {
                    _referenceList.Remove(_selectedReference);
                    SelectedReference = null;
                },
                () => _selectedReference != null);
        }

        public DelegateCommand AddFileCommand
        {
            get { return _addFile; }
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

        public bool CanChangeTech
        {
            get
            {
                return _principal.IsInRole(UserRoleNames.ReportAdmin); 
            }
        }

        public DelegateCommand<Window> ConfirmCommand
        {
            get { return _confirm; }
        }

        public ObservableCollection<CalibrationFiles> FileList
        {
            get { return _calibrationFileList; }
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
                RaisePropertyChanged("InstrumentCode");
            }
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

        public List<Organization> LabList
        {
            get
            {
                OrganizationRole tempOr = _entities.OrganizationRoles.First(orr => orr.Name == DBManager.OrganizationRoleNames.CalibrationLab);
                return new List<Organization>(tempOr.OrganizationMappings.Where(orm => orm.IsSelected)
                                                .OrderBy(orm => orm.Organization.Name)
                                                .Select(orm => orm.Organization));
            }
        }

        public DelegateCommand OpenFileCommand
        {
            get { return _openFile; }
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

        public Int32 ReportNumber
        {
            get { return _reportNumber; }
            set { _reportNumber = value; }
        }

        public DelegateCommand RemoveFileCommand
        {
            get { return _removeFile; }
        }

        public CalibrationFiles SelectedFile
        {
            get { return _selectedFile; }
            set
            {
                _selectedFile = value;
                RaisePropertyChanged("SelectedFile");
                _openFile.RaiseCanExecuteChanged();
                _removeFile.RaiseCanExecuteChanged();
            }
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
