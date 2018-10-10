using Controls.Views;
using DataAccess;
using Infrastructure;
using Infrastructure.Commands;
using Infrastructure.Events;
using Infrastructure.Queries;
using Instruments.Queries;
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
using System.Windows;
using System.Windows.Forms;

namespace Instruments.ViewModels
{
    public class CalibrationReportEditViewModel : BindableBase
    {
        #region Fields

        private CalibrationReport _calibrationInstance;
        
        private bool _editMode,
                                            _readOnlyMode;

        private IEventAggregator _eventAggregator;
        private InstrumentService _instrumentService;
        private IDataService<LabDbEntities> _labDbData;
        private string _referenceCode;
        private CalibrationFiles _selectedFile;
        private Organization _selectedLab;
        private Person _selectedPerson;
        private Instrument _selectedReference;
        private CalibrationResult _selectedResult;
        private DelegateCommand _startEdit;

        #endregion Fields

        #region Constructors

        public CalibrationReportEditViewModel(IDataService<LabDbEntities> labDbData,
                                                IEventAggregator eventAggregator,
                                                InstrumentService instrumentService)
        {
            _labDbData = labDbData;
            _editMode = false;
            _instrumentService = instrumentService;

            _eventAggregator = eventAggregator;

            LabList = _labDbData.RunQuery(new OrganizationsQuery() { Role = OrganizationsQuery.OrganizationRoles.CalibrationLab })
                                .ToList();
            CalibrationResultList = _instrumentService.GetCalibrationResults();
            TechList = _labDbData.RunQuery(new PeopleQuery() { Role = PeopleQuery.PersonRoles.CalibrationTech })
                                                            .ToList();

            AddFileCommand = new DelegateCommand(
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

                        _instrumentService.AddCalibrationFiles(fileList);
                        RaisePropertyChanged("FileList");
                    }
                });

            AddReferenceCommand = new DelegateCommand<string>(
                code =>
                {
                    Instrument tempRef = _labDbData.RunQuery(new InstrumentQuery() { Code = code });
                    if (tempRef != null)
                    {
                        _calibrationInstance.AddReference(tempRef);
                        ReferenceCode = "";
                        RaisePropertyChanged("ReferenceList");
                    }
                });

            CancelEditCommand = new DelegateCommand(
                () =>
                {
                    CalibrationInstance = _labDbData.RunQuery(new CalibrationReportsQuery()).FirstOrDefault(crep => crep.ID == _calibrationInstance.ID);
                },
                () => EditMode);

            DeleteCommand = new DelegateCommand(
                () =>
                {
                    _labDbData.Execute(new DeleteEntityCommand(_calibrationInstance));
                },
                () => Thread.CurrentPrincipal.IsInRole(UserRoleNames.InstrumentAdmin));

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

            RemoveReferenceCommand = new DelegateCommand(
                () =>
                {
                    _calibrationInstance.RemoveReference(SelectedReference);
                    SelectedReference = null;
                    RaisePropertyChanged("ReferenceList");
                },
                () => SelectedReference != null);

            SaveCommand = new DelegateCommand(
                () =>
                {
                    _labDbData.Execute(new UpdateEntityCommand(_calibrationInstance));
                    foreach (CalibrationReportInstrumentPropertyMapping cripmw in PropertyMappingList)
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

        #endregion Constructors

        #region Properties

        public DelegateCommand AddFileCommand { get; }

        public DelegateCommand<string> AddReferenceCommand { get; }

        public CalibrationReport CalibrationInstance
        {
            get { return _calibrationInstance; }
            set
            {
                EditMode = false;
                _calibrationInstance = value;
                _calibrationInstance.Load();

                PropertyMappingList = _calibrationInstance?.GetPropertyMappings();

                _selectedLab = LabList.FirstOrDefault(lab => lab.ID == _calibrationInstance?.laboratoryID);
                _selectedResult = CalibrationResultList.FirstOrDefault(res => res.ID == _calibrationInstance?.ResultID);
                _selectedPerson = TechList.FirstOrDefault(tech => tech.ID == _calibrationInstance?.OperatorID);

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

        public IEnumerable<CalibrationResult> CalibrationResultList { get; }
        public DelegateCommand CancelEditCommand { get; }
        public DelegateCommand DeleteCommand { get; }

        public bool EditMode
        {
            get { return _editMode; }
            set
            {
                _editMode = value;
                ReadOnlyMode = !value;
                RaisePropertyChanged("EditMode");
                RaisePropertyChanged("TechSelectionEnabled");
                CancelEditCommand.RaiseCanExecuteChanged();
                SaveCommand.RaiseCanExecuteChanged();
                _startEdit.RaiseCanExecuteChanged();
            }
        }

        public IEnumerable<CalibrationFiles> FileList => _calibrationInstance.GetFiles();

        public string FileListRegionName => RegionNames.CalibrationEditFileListRegion;

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

        public IEnumerable<Organization> LabList { get; }

        public DelegateCommand OpenFileCommand { get; }

        public IEnumerable<CalibrationReportInstrumentPropertyMapping> PropertyMappingList { get; private set; }

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

        public IEnumerable<Instrument> ReferenceList => _calibrationInstance.GetReferenceInstruments();

        public DelegateCommand RemoveFileCommand { get; }

        public DelegateCommand RemoveReferenceCommand { get; }

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

        public DelegateCommand SaveCommand { get; }

        public CalibrationFiles SelectedFile
        {
            get { return _selectedFile; }
            set
            {
                _selectedFile = value;
                RemoveFileCommand.RaiseCanExecuteChanged();
                OpenFileCommand.RaiseCanExecuteChanged();

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
                RemoveReferenceCommand.RaiseCanExecuteChanged();
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

        public DelegateCommand StartEditCommand => _startEdit;

        public IEnumerable<Person> TechList { get; }

        public bool TechSelectionEnabled => EditMode && _selectedLab.Name == "Vulcaflex";

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

        #endregion Properties
    }
}