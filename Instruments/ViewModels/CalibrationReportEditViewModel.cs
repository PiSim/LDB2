using Controls.Views;
using DataAccessCore;
using DataAccessCore.Commands;
using Infrastructure;
using Infrastructure.Commands;
using Infrastructure.Events;
using Infrastructure.Queries;
using Instruments.Queries;
using LInst;
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
        private IDataService<LInstContext> _lInstData;
        private string _referenceCode;
        private CalibrationFile _selectedFile;
        private Organization _selectedLab;
        private Person _selectedPerson;
        private Instrument _selectedReference;
        private CalibrationResult _selectedResult;
        private DelegateCommand _startEdit;

        #endregion Fields

        #region Constructors

        public CalibrationReportEditViewModel(IDataService<LInstContext> lInstData,
                                                IEventAggregator eventAggregator,
                                                InstrumentService instrumentService)
        {
            _lInstData = lInstData;
            _editMode = false;
            _instrumentService = instrumentService;

            _eventAggregator = eventAggregator;


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
                        IEnumerable<CalibrationFile> fileList = fileDialog.FileNames
                                                                            .Select(file => new CalibrationFile()
                                                                            {
                                                                                CalibrationReportID = _calibrationInstance.ID,
                                                                                Path = file
                                                                            });

                        _lInstData.Execute(new BulkInsertEntitiesCommand<LInstContext>(fileList));
                        RaisePropertyChanged("FileList");
                    }
                });

            AddReferenceCommand = new DelegateCommand<string>(
                code =>
                {
                    Instrument tempRef = _lInstData.RunQuery(new InstrumentQuery() { Code = code });
                    if (tempRef != null)
                    {
                        _lInstData.Execute(new InsertEntityCommand<LInstContext>(
                            new CalibrationReportReference()
                            {
                                CalibrationReportID = _calibrationInstance.ID,
                                InstrumentID = tempRef.ID
                            }));
                        ReferenceCode = "";
                        RaisePropertyChanged("ReferenceList");
                    }
                });

            CancelEditCommand = new DelegateCommand(
                () =>
                {
                    CalibrationInstance = _lInstData.RunQuery(new CalibrationReportsQuery()).FirstOrDefault(crep => crep.ID == _calibrationInstance.ID);
                },
                () => EditMode);

            DeleteCommand = new DelegateCommand(
                () =>
                {
                    _lInstData.Execute(new DeleteEntityCommand<LInstContext>(_calibrationInstance));
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
                    _lInstData.Execute(new DeleteEntityCommand<LInstContext>(_selectedFile));

                    RaisePropertyChanged("FileList");

                    SelectedFile = null;
                },
                () => _selectedFile != null);

            RemoveReferenceCommand = new DelegateCommand(
                () =>
                {
                    _lInstData.Execute(new DeleteEntityCommand<LInstContext>(SelectedReference));
                    SelectedReference = null;
                    RaisePropertyChanged("ReferenceList");
                },
                () => SelectedReference != null);

            SaveCommand = new DelegateCommand(
                () =>
                {
                    _lInstData.Execute(new UpdateEntityCommand<LInstContext>(_calibrationInstance));

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

        #region Methods

        private void RefreshCollections()
        {
            LabList = _lInstData.RunQuery(new OrganizationsQuery() { Role = OrganizationsQuery.OrganizationRoles.CalibrationLab })
                                .ToList();
            CalibrationResultList = _lInstData.RunQuery(new CalibrationResultsQuery()).ToList();
            TechList = _lInstData.RunQuery(new PeopleQuery() { Role = PeopleQuery.PersonRoles.CalibrationTech })
                                                            .ToList();

            RaisePropertyChanged("LabList");
            RaisePropertyChanged("CalibrationResultList");
            RaisePropertyChanged("TechList");
        }

        #endregion

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

                RefreshCollections();

                _selectedLab = LabList.FirstOrDefault(lab => lab.ID == _calibrationInstance?.LaboratoryID);
                _selectedResult = CalibrationResultList.FirstOrDefault(res => res.ID == _calibrationInstance?.CalibrationResultID);
                _selectedPerson = TechList.FirstOrDefault(tech => tech.ID == _calibrationInstance?.TechID);

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

        public IEnumerable<CalibrationResult> CalibrationResultList { get; private set; }
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

        public IEnumerable<CalibrationFile> FileList => (_calibrationInstance == null) ? new List<CalibrationFile>() : _lInstData.RunQuery(new CalibrationFilesQuery() { CalibrationReportID = _calibrationInstance.ID }).ToList();

        public string FileListRegionName => RegionNames.CalibrationEditFileListRegion;

        public IEnumerable<Organization> LabList { get; private set; }

        public DelegateCommand OpenFileCommand { get; }
        
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

        public IEnumerable<Instrument> ReferenceList => (_calibrationInstance == null) ? new List<Instrument>() : _lInstData.RunQuery(new ReferenceInstrumentsQuery(_calibrationInstance)).ToList();

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

        public CalibrationFile SelectedFile
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
                if (_calibrationInstance != null && value != null)
                {
                    _calibrationInstance.Laboratory = value;
                    _calibrationInstance.LaboratoryID = _selectedLab.ID;
                }
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
                if (_calibrationInstance != null && value != null)
                {
                    _calibrationInstance.CalibrationResult = value;
                    _calibrationInstance.CalibrationResultID = value.ID;

                }
            }
        }

        public Person SelectedTech
        {
            get { return _selectedPerson; }
            set
            {
                _selectedPerson = value;
                if (_calibrationInstance != null && value != null)
                {
                    _calibrationInstance.Tech = value;
                    _calibrationInstance.TechID = value.ID;

                }
            }
        }

        public DelegateCommand StartEditCommand => _startEdit;

        public IEnumerable<Person> TechList { get; private set; }

        public bool TechSelectionEnabled => EditMode && _selectedLab.Name == "Vulcaflex";
        
        #endregion Properties
    }
}