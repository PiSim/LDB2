using DataAccess;
using Infrastructure;
using Infrastructure.Commands;
using Infrastructure.Events;
using Infrastructure.Queries;
using LabDbContext;
using LabDbContext.EntityExtensions;
using LabDbContext.Services;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Reporting;
using Reports.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Reports.ViewModels
{
    public class ReportEditViewModel : BindableBase
    {
        #region Fields

        private IEventAggregator _eventAggregator;
        private Report _instance;
        private IDataService<LabDbEntities> _labDbData;

        private bool _projectChanged,
                                                     _editMode;

        private IEnumerable<Project> _projectList;
        private IReportingService _reportingService;
        private IReportService _reportService;
        private DelegateCommand _save;
        private ReportFile _selectedFile;
        private Project _selectedProject;
        private IEnumerable<TestWrapper> _testList;

        #endregion Fields

        #region Constructors

        public ReportEditViewModel(IEventAggregator aggregator,
                                    IDataService<LabDbEntities> labDbData,
                                    IReportService reportService,
                                    IReportingService reportingService) : base()
        {
            _labDbData = labDbData;
            _editMode = false;
            _eventAggregator = aggregator;
            _projectChanged = false;
            _reportService = reportService;
            _reportingService = reportingService;

            AddFileCommand = new DelegateCommand(
                () =>
                {
                    OpenFileDialog fileDialog = new OpenFileDialog
                    {
                        InitialDirectory = UserSettings.ReportPath,
                        Multiselect = true
                    };

                    if (fileDialog.ShowDialog() == DialogResult.OK)
                    {
                        foreach (string pth in fileDialog.FileNames)
                        {
                            ReportFile temp = new ReportFile();
                            temp.Path = pth;
                            temp.Description = "";
                            temp.reportID = _instance.ID;
                            _labDbData.Execute(new InsertEntityCommand(temp));
                        }

                        RaisePropertyChanged("FileList");
                    }
                },
                () => CanModify);

            AddTestsCommand = new DelegateCommand(
                () =>
                {
                    if (_reportService.AddTestsToReport(_instance))
                        TestList = new List<TestWrapper>(  _instance.TestRecord.Tests.Select(tst => new TestWrapper(tst)));
                },
                () => CanModify);

            GenerateRawDataSheetCommand = new DelegateCommand(
                () =>
                {
                    _reportingService.PrintReportDataSheet(_instance);
                });

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
                    SelectedFile = null;

                    RaisePropertyChanged("FileList");
                },
                () => CanModify && _selectedFile != null);

            RemoveTestCommand = new DelegateCommand<Test>(
                testItem =>
                {
                    _labDbData.Execute(new DeleteEntityCommand(testItem));

                    TestList = new List<TestWrapper>(_instance.TestRecord.Tests.Select(tst => new TestWrapper(tst)));
                },
                testItem => EditMode);

            _save = new DelegateCommand(
                () =>
                {
                    // Update the tests

                    _labDbData.Execute(new BulkUpdateSubTestResultsCommand(_testList.SelectMany(tstw => tstw.TestInstance.SubTests)));

                    // Update the report instance

                    _labDbData.Execute(new UpdateEntityCommand(_instance));

                    // If the project was modified, update the material

                    if (_projectChanged)
                        _instance.SetProject(_selectedProject);

                    EditMode = false;
                },
                () => _editMode);

            StartEditCommand = new DelegateCommand(
                () =>
                {
                    EditMode = true;
                },
                () => !_editMode && CanModify);

            #region EventSubscriptions

            _eventAggregator.GetEvent<ProjectChanged>()
                            .Subscribe(ect =>
                            {
                                _projectList = null;
                                RaisePropertyChanged("ProjectList");
                            });

            #endregion EventSubscriptions
        }

        #endregion Constructors

        #region Properties

        public DelegateCommand AddFileCommand { get; }

        public DelegateCommand AddTestsCommand { get; }

        public string BatchNumber
        {
            get
            {
                if (_instance == null)
                    return null;
                else
                    return _instance.Batch.Number;
            }
        }

        public bool CanModify
        {
            get
            {
                if (_instance == null)
                    return false;
                else if (Thread.CurrentPrincipal.IsInRole(UserRoleNames.ReportEdit))
                    return true;
                else
                    return false;
            }
        }

        public string Category
        {
            get
            {
                if (_instance == null)
                    return null;
                else
                    return _instance.Category;
            }
        }

        public string Description
        {
            get
            {
                if (_instance == null)
                    return null;
                else
                    return _instance.Description;
            }
            set { _instance.Description = value; }
        }

        public bool EditMode
        {
            get { return _editMode; }
            set
            {
                _editMode = value;
                RaisePropertyChanged("EditMode");
                RaisePropertyChanged("ReadOnlyMode");

                RemoveTestCommand.RaiseCanExecuteChanged();
                _save.RaiseCanExecuteChanged();
                StartEditCommand.RaiseCanExecuteChanged();
            }
        }

        public IEnumerable<ReportFile> FileList => _instance.GetReportFiles();

        public DelegateCommand GenerateRawDataSheetCommand { get; }

        public Report Instance
        {
            get { return _instance; }
            set
            {
                EditMode = false;
                _projectChanged = false;

                _instance = value;

                _testList = new List<TestWrapper>(_instance.TestRecord.Tests.Select(tst => new TestWrapper(tst)));
                _selectedProject = _instance?.GetProject();

                AddFileCommand.RaiseCanExecuteChanged();
                AddTestsCommand.RaiseCanExecuteChanged();
                RemoveFileCommand.RaiseCanExecuteChanged();
                RemoveTestCommand.RaiseCanExecuteChanged();
                StartEditCommand.RaiseCanExecuteChanged();

                RaisePropertyChanged("BatchNumber");
                RaisePropertyChanged("CanModify");
                RaisePropertyChanged("Category");
                RaisePropertyChanged("Description");
                RaisePropertyChanged("FileList");
                RaisePropertyChanged("Material");
                RaisePropertyChanged("Number");
                RaisePropertyChanged("SelectedProject");
                RaisePropertyChanged("SpecificationName");
                RaisePropertyChanged("SpecificationVersion");
                RaisePropertyChanged("TestList");
                RaisePropertyChanged("TotalDuration");
            }
        }

        public Material Material
        {
            get
            {
                if (_instance == null)
                    return null;
                else
                    return _instance.Batch.Material;
            }
        }

        public string Number
        {
            get
            {
                if (_instance == null)
                    return null;
                else
                    return _instance.Number.ToString();
            }
        }

        public DelegateCommand OpenFileCommand { get; }

        public IEnumerable<Project> ProjectList
        {
            get
            {
                if (_projectList == null)
                    _projectList = _labDbData.RunQuery(new ProjectsQuery())
                                            .ToList();

                return _projectList;
            }
        }

        public bool ReadOnlyMode => !EditMode;

        public DelegateCommand RemoveFileCommand { get; }

        public DelegateCommand<Test> RemoveTestCommand { get; }

        public DelegateCommand SaveCommand => _save;

        public ReportFile SelectedFile
        {
            get { return _selectedFile; }
            set
            {
                _selectedFile = value;
                OpenFileCommand.RaiseCanExecuteChanged();
                RemoveFileCommand.RaiseCanExecuteChanged();

                RaisePropertyChanged("SelectedFile");
            }
        }

        public Project SelectedProject
        {
            get
            {
                return _projectList.FirstOrDefault(prj => prj.ID == _selectedProject?.ID);
            }

            set
            {
                _selectedProject = value;
                _projectChanged = true;
            }
        }

        public string SpecificationName => _instance?.SpecificationVersion?.Specification.Name;

        public string SpecificationVersion
        {
            get
            {
                if (_instance == null)
                    return null;
                else
                    return _instance.SpecificationVersion.Name;
            }
        }

        public DelegateCommand StartEditCommand { get; }

        public IEnumerable<TestWrapper> TestList
        {
            get { return _testList; }
            private set
            {
                _testList = value;
                RaisePropertyChanged("TestList");
            }
        }

        public double TotalDuration => (_instance == null) ? 0 : _instance.TotalDuration;

        #endregion Properties
    }
}