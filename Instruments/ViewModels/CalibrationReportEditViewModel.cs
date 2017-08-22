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
using System.Windows.Forms;

namespace Instruments.ViewModels
{
    public class CalibrationReportEditViewModel : BindableBase
    {
        private bool _editMode;
        private CalibrationFiles _selectedFile;
        private CalibrationReport _calibrationInstance;
        private DBPrincipal _principal;
        private DelegateCommand _addFile,
                                _delete,
                                _openFile,
                                _removeFile,
                                _save,
                                _startEdit;
        private EventAggregator _eventAggregator;
        private IEnumerable<Organization> _labList;
        private IEnumerable<Person> _techList;
        private Organization _selectedLab;
        private Person _selectedPerson;
        
        public CalibrationReportEditViewModel(DBPrincipal principal,
                                                EventAggregator eventAggregator)
        {
            _editMode = false;
            _principal = principal;

            _labList = OrganizationService.GetOrganizations(OrganizationRoleNames.CalibrationLab);
            _techList = PeopleService.GetPeople(PersonRoleNames.CalibrationTech);

            _addFile = new DelegateCommand(
                () =>
                {
                    OpenFileDialog fileDialog = new OpenFileDialog();
                    fileDialog.Multiselect = true;

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

            _save = new DelegateCommand(
                () =>
                {
                    _calibrationInstance.Update();
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

        public CalibrationReport CalibrationInstance
        {
            get { return _calibrationInstance; }
            set
            {
                EditMode = false;
                _calibrationInstance = value;

                SelectedLab = _labList.FirstOrDefault(lab => lab.ID == _calibrationInstance?.laboratoryID);
                SelectedTech = _techList.FirstOrDefault(tech => tech.ID == _calibrationInstance?.OperatorID);

                RaisePropertyChanged("SelectedLab");
                RaisePropertyChanged("SelectedTech");

                RaisePropertyChanged("FileList");
                SelectedFile = null;

                RaisePropertyChanged("CalibrationInstance");
                RaisePropertyChanged("ReportViewVisibility");
                RaisePropertyChanged("PropertyMappingList");
            }
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
                RaisePropertyChanged("EditMode");
                RaisePropertyChanged("TechSelectionEnabled");
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
            get { return _calibrationInstance.GetPropertyMappings(); }
        }

        public IEnumerable<Instrument> ReferenceList
        {
            get { return _calibrationInstance.GetReferenceInstruments(); }
        }

        public DelegateCommand RemoveFileCommand
        {
            get { return _removeFile; }
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
            }
        }

        public Person SelectedTech
        {
            get { return _selectedPerson; }
            set
            {
                _selectedPerson = value;
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
    }
}
