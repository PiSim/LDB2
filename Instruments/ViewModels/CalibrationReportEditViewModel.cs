using DBManager;
using DBManager.EntityExtensions;
using DBManager.Services;
using Infrastructure;
using Infrastructure.Events;
using Infrastructure.Wrappers;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace Instruments.ViewModels
{
    public class CalibrationReportEditViewModel : BindableBase
    {
        private bool _editMode,
                    _readOnlyMode;
        private CalibrationFiles _selectedFile;
        private CalibrationReport _calibrationInstance;
        private CalibrationResult _selectedResult;
        private DBPrincipal _principal;
        private DelegateCommand _addFile,
                                _cancelEdit,
                                _delete,
                                _openFile,
                                _removeFile,
                                _removeReference,
                                _save,
                                _startEdit;
        private DelegateCommand<string> _addReference;
        private EventAggregator _eventAggregator;
        private IEnumerable<CalibrationReportInstrumentPropertyMapping> _mappingList;
        private IEnumerable<CalibrationResult> _resultList;
        private IEnumerable<Organization> _labList;
        private IEnumerable<Person> _techList;
        private Instrument _selectedReference;
        private Organization _selectedLab;
        private Person _selectedPerson;
        private string _referenceCode;

        public CalibrationReportEditViewModel(DBPrincipal principal,
                                                EventAggregator eventAggregator)
        {
            _editMode = false;
            _principal = principal;
            _eventAggregator = eventAggregator;

            _labList = OrganizationService.GetOrganizations(OrganizationRoleNames.CalibrationLab);
            _resultList = InstrumentService.GetCalibrationResults();
            _techList = PeopleService.GetPeople(PersonRoleNames.CalibrationTech);

            _addFile = new DelegateCommand(
                () =>
                {
                    
                    OpenFileDialog fileDialog = new OpenFileDialog
                    {
                        InitialDirectory = UserSettings.CalibrationReportPath,
                        Multiselect = true
                    };

                    if (fileDialog.ShowDialog() == DialogResult.OK)
                    {


                        IEnumerable<CalibrationFiles> fileList = fileDialog.FileNames
                                                                            .Select(file => new CalibrationFiles()
                                                                            {
                                                                                ReportID = _calibrationInstance.ID,
                                                                                Path = file,
                                                                                Description = ""
                                                                            });

                        InstrumentService.AddCalibrationFiles(fileList);
                        RaisePropertyChanged("FileList");
                    }
                });

            _addReference = new DelegateCommand<string>(
                code =>
                {
                    Instrument tempRef = InstrumentService.GetInstrument(code);
                    if (tempRef != null)
                    {
                        _calibrationInstance.AddReference(tempRef);
                        ReferenceCode = "";
                        RaisePropertyChanged("ReferenceList");
                    }
                });

            _cancelEdit = new DelegateCommand(
                () =>
                {
                    CalibrationInstance = InstrumentService.GetCalibrationReport(_calibrationInstance.ID);
                },
                () => EditMode);

            _delete = new DelegateCommand(
                () =>
                {
                    _calibrationInstance.Delete();
                },
                () => _principal.IsInRole(UserRoleNames.InstrumentAdmin));

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
                    _selectedFile.Delete();

                    RaisePropertyChanged("FileList");

                    SelectedFile = null;
                },
                () => _selectedFile != null);

            _removeReference = new DelegateCommand(
                () =>
                {
                    _calibrationInstance.RemoveReference(SelectedReference);
                    SelectedReference = null;
                    RaisePropertyChanged("ReferenceList");
                },
                () => SelectedReference != null);

            _save = new DelegateCommand(
                () =>
                {
                    _calibrationInstance.Update();
                    foreach (CalibrationReportInstrumentPropertyMapping cripmw in _mappingList)
                        cripmw.Update();

                    EditMode = false;
                },
                () => EditMode);

            _startEdit = new DelegateCommand(
                () =>
                {
                    EditMode = true;
                },
                () => !EditMode);
        }

        public DelegateCommand AddFileCommand
        {
            get { return _addFile; }
        }

        public DelegateCommand<string> AddReferenceCommand
        {
            get { return _addReference; }
        }


        public CalibrationReport CalibrationInstance
        {
            get { return _calibrationInstance; }
            set
            {
                EditMode = false;
                _calibrationInstance = value;
                _calibrationInstance.Load();

                _mappingList = _calibrationInstance?.GetPropertyMappings();

                _selectedLab = _labList.FirstOrDefault(lab => lab.ID == _calibrationInstance?.laboratoryID);
                _selectedResult = _resultList.FirstOrDefault(res => res.ID == _calibrationInstance?.ResultID);
                _selectedPerson = _techList.FirstOrDefault(tech => tech.ID == _calibrationInstance?.OperatorID);

                RaisePropertyChanged("SelectedLab");
                RaisePropertyChanged("SelectedResult");
                RaisePropertyChanged("SelectedTech");

                RaisePropertyChanged("FileList");
                SelectedFile = null;
                SelectedReference = null;

                RaisePropertyChanged("CalibrationInstance");
                RaisePropertyChanged("IsVerification");
                RaisePropertyChanged("ReferenceList");
                RaisePropertyChanged("ReportViewVisibility");
                RaisePropertyChanged("PropertyMappingList");
                RaisePropertyChanged("UncertaintyHeader");
            }
        }

        public DelegateCommand CancelEditCommand
        {
            get { return _cancelEdit; }
        }

        public IEnumerable<CalibrationResult> CalibrationResultList
        {
            get { return _resultList; }
        }

        public DelegateCommand DeleteCommand
        {
            get { return _delete; }
        }

        public bool EditMode
        {
            get { return _editMode; }
            set
            {
                _editMode = value;
                ReadOnlyMode = !value;
                RaisePropertyChanged("EditMode");
                RaisePropertyChanged("TechSelectionEnabled");
                _cancelEdit.RaiseCanExecuteChanged();
                _save.RaiseCanExecuteChanged();
                _startEdit.RaiseCanExecuteChanged();
            }
        }

        public IEnumerable<CalibrationFiles> FileList
        {
            get { return _calibrationInstance.GetFiles(); }
        }

        public string FileListRegionName
        {
            get { return RegionNames.CalibrationEditFileListRegion; }
        }

        public bool IsVerification
        {
            get
            {
                if (_calibrationInstance == null)
                    return false;

                else
                    return _calibrationInstance.IsVerification;
            }
            set
            {
                _calibrationInstance.IsVerification = value;
                RaisePropertyChanged("UncertaintyHeader");
            }
        }

        public IEnumerable<Organization> LabList
        {
            get { return _labList; }
        }

        public DelegateCommand OpenFileCommand
        {
            get { return _openFile; }
        }

        public IEnumerable<CalibrationReportInstrumentPropertyMapping> PropertyMappingList
        {
            get
            {
                return _mappingList;
            }
        }

        public bool ReadOnlyMode
        {
            get { return _readOnlyMode; }
            set
            {
                _readOnlyMode = value;
                RaisePropertyChanged("ReadOnlyMode");
            }
        }

        public string ReferenceCode
        {
            get { return _referenceCode; }
            set
            {
                _referenceCode = value;
                RaisePropertyChanged("ReferenceCode");
            }
        }

        public IEnumerable<Instrument> ReferenceList
        {
            get { return _calibrationInstance.GetReferenceInstruments(); }
        }

        public DelegateCommand RemoveFileCommand
        {
            get { return _removeFile; }
        }

        public DelegateCommand RemoveReferenceCommand
        {
            get { return _removeReference; }
        }

        public Visibility ReportViewVisibility
        {
            get
            {
                if (_calibrationInstance == null)
                    return Visibility.Hidden;

                else
                    return Visibility.Visible;
            }
        }

        public DelegateCommand SaveCommand
        {
            get { return _save; }
        }

        public CalibrationFiles SelectedFile
        {
            get { return _selectedFile; }
            set
            {
                _selectedFile = value;
                _removeFile.RaiseCanExecuteChanged();
                _openFile.RaiseCanExecuteChanged();

                RaisePropertyChanged("SelectedFile");
            }
        }

        public Organization SelectedLab
        {
            get { return _selectedLab; }
            set
            {
                _selectedLab = value;
                _calibrationInstance.laboratoryID = _selectedLab.ID;
            }
        }

        public Instrument SelectedReference
        {
            get { return _selectedReference; }
            set
            {
                _selectedReference = value;
                RaisePropertyChanged("SelectedReference");
                _removeReference.RaiseCanExecuteChanged();
            }
        }

        public CalibrationResult SelectedResult
        {
            get { return _selectedResult; }
            set
            {
                _selectedResult = value;
                _calibrationInstance.ResultID = value.ID;
            }
        }

        public Person SelectedTech
        {
            get { return _selectedPerson; }
            set
            {
                _selectedPerson = value;
                _calibrationInstance.OperatorID = value.ID;
            }
        }

        public DelegateCommand StartEditCommand
        {
            get { return _startEdit; }
        }

        public IEnumerable<Person> TechList
        {
            get { return _techList; }
        }

        public bool TechSelectionEnabled
        {
            get { return EditMode && _selectedLab.Name == "Vulcaflex"; }
        }

        public string UncertaintyHeader
        {
            get
            {
                if (IsVerification)
                    return "Scarto Max";

                else
                    return "Incertezza Estesa";
            }
        }
    }
}
