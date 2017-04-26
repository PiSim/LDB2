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
using System.Windows.Forms;

namespace Instruments.ViewModels
{
    public class NewCalibrationDialogViewModel : BindableBase
    {
        private CalibrationFiles _selectedFile;
        private DateTime _calibrationDate;
        private DBEntities _entities;
        private DBPrincipal _principal;
        private DelegateCommand _addFile, _cancel, _confirm, _openFile, _removeFile;
        private EventAggregator _eventAggregator;
        private Instrument _instumentInstance;
        private Person _selectedTech;
        private string _calibrationNotes, _calibrationResult, _referenceCode;
        private ObservableCollection<CalibrationFiles> _calibrationFileList;
        private Organization _selectedLab;
        private Views.NewCalibrationDialog _parentDialog;

        public NewCalibrationDialogViewModel(DBEntities entities,
                                            DBPrincipal principal,
                                            EventAggregator eventAggregator,
                                            Views.NewCalibrationDialog parentDialog) : base()
        {
            _entities = entities;
            _calibrationFileList = new ObservableCollection<CalibrationFiles>();
            _eventAggregator = eventAggregator;
            _principal = principal;
            _parentDialog = parentDialog;

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

            _cancel = new DelegateCommand(
                () =>
                {
                    parentDialog.DialogResult = false;
                });

            _confirm = new DelegateCommand(
                () =>
                {
                    CalibrationReport output = new CalibrationReport();
                    output.Date = _calibrationDate;
                    output.Instrument = _instumentInstance;
                    output.Laboratory = _selectedLab;

                    if (IsNotExternalLab)
                    {
                        output.Tech = _selectedTech;
                        Instrument tempReference = _entities.Instrument.FirstOrDefault(prm => prm.Code == _referenceCode);
                        if (tempReference == null)
                            return;
                        output.Reference = tempReference;
                    }

                    foreach (CalibrationFiles calFile in _calibrationFileList)
                        output.CalibrationFiles.Add(calFile);

                    _entities.CalibrationReports.Add(output);
                    _entities.SaveChanges();

                    _parentDialog.ReportInstance = output;
                    _parentDialog.DialogResult = true;
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
                },
                () => _selectedFile != null);
        }

        public DelegateCommand AddFileCommand
        {
            get { return _addFile; }
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

        public DelegateCommand CancelCommand
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

        public DelegateCommand ConfirmCommand
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
                OrganizationRole tempOr = _entities.OrganizationRoles.First(orr => orr.Name == OrganizationRoleNames.CalibrationLab);
                return new List<Organization>(tempOr.OrganizationMappings.Where(orm => orm.IsSelected)
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
            }
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
