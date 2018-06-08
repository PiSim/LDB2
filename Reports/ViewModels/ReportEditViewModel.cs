using DBManager;
using DBManager.EntityExtensions;
using DBManager.Services;
using Infrastructure;
using Infrastructure.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Reporting;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Reports.ViewModels
{
    public class ReportEditViewModel : BindableBase
    {
        private bool _projectChanged,
                     _editMode;
        private DBPrincipal _principal;
        private DelegateCommand _addFile, 
                                _addTests, 
                                _generateRawDataSheet, 
                                _openFile, 
                                _removeFile,
                                _save,
                                _startEdit;
        private DelegateCommand<Test> _removeTest;
        private EventAggregator _eventAggregator;
        private Report _instance;
        private IDataService _dataService;
        private IEnumerable<Project> _projectList;
        private IEnumerable<TestWrapper> _testList;
        private IReportService _reportService;
        private IReportingService _reportingService;
        private Project _selectedProject;
        private ReportFile _selectedFile;
        
        public ReportEditViewModel(DBPrincipal principal,
                                    EventAggregator aggregator,
                                    IDataService dataService,
                                    IReportService reportService,
                                    IReportingService reportingService) : base()
        {
            _dataService = dataService;
            _editMode = false;
            _eventAggregator = aggregator;
            _principal = principal;
            _projectChanged = false;
            _reportService = reportService;
            _reportingService = reportingService;

            _addFile = new DelegateCommand(
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
                            temp.Create();
                        }

                        RaisePropertyChanged("FileList");
                    }
                },
                () => CanModify);

            _addTests = new DelegateCommand(
                () =>
                {
                    if (_reportService.AddTestsToReport(_instance))
                    {
                        TestList = new List<TestWrapper>(_instance.GetTests().Select(tst => new TestWrapper(tst)));
                        _eventAggregator.GetEvent<ReportStatusCheckRequested>()
                                        .Publish(_instance);
                    }
                },
                () => CanModify);

            _generateRawDataSheet = new DelegateCommand(
                () =>
                {
                    _reportingService.PrintReportDataSheet(_instance);
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
                    _selectedFile.Delete();
                    SelectedFile = null;

                    RaisePropertyChanged("FileList");
                },
                () => CanModify && _selectedFile != null);

            _removeTest = new DelegateCommand<Test>(
                testItem =>
                {
                    TaskItem tempTaskItem = testItem.GetTaskItem();
                    testItem.Delete();

                    if (tempTaskItem != null)
                    {
                        tempTaskItem = _dataService.GetTaskItem(tempTaskItem.ID);
                        tempTaskItem.Update();
                    }

                    _eventAggregator.GetEvent<ReportStatusCheckRequested>()
                                    .Publish(_instance);

                    TestList = new List<TestWrapper>(_instance.GetTests().Select(tst => new TestWrapper(tst)));

                },
                testItem => CanModify);

            _save = new DelegateCommand(
                () =>
                {
                    // Update the tests
                    _testList.Select(tiw => tiw.TestInstance)
                            .Update();

                    // Update the report instance

                    _instance.Update();

                    // If the project was modified, update the material

                    if (_projectChanged)
                        _instance.SetProject(_selectedProject);

                    EditMode = false;
                    

                    _eventAggregator.GetEvent<ReportStatusCheckRequested>().Publish(_instance);
                },
                () => _editMode);

            _startEdit = new DelegateCommand(
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

            #endregion
        }

        public DelegateCommand AddFileCommand
        {
            get { return _addFile; }
        }

        public DelegateCommand AddTestsCommand
        {
            get { return _addTests; }
        }

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

                else if (_principal.IsInRole(UserRoleNames.ReportEdit))
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

                _save.RaiseCanExecuteChanged();
                _startEdit.RaiseCanExecuteChanged();
            }
        }

        public IEnumerable<ReportFile> FileList
        {
            get 
            {
                return _instance.GetReportFiles();
            }
        }

        public DelegateCommand GenerateRawDataSheetCommand
        {
            get { return _generateRawDataSheet; }
        }

        public Report Instance
        {
            get { return _instance; }
            set
            {
                EditMode = false;
                _projectChanged = false;

                _instance = value;
                _instance.Load();

                _testList = new List<TestWrapper>(_instance.GetTests().Select(tst => new TestWrapper(tst)));
                _selectedProject = _instance?.GetProject();

                _addFile.RaiseCanExecuteChanged();
                _addTests.RaiseCanExecuteChanged();
                _removeFile.RaiseCanExecuteChanged();
                _removeTest.RaiseCanExecuteChanged();
                _startEdit.RaiseCanExecuteChanged();

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

        public DelegateCommand OpenFileCommand
        {
            get { return _openFile; }
        }

        public IEnumerable<Project> ProjectList
        {
            get
            {
                if (_projectList == null)
                    _projectList = _dataService.GetProjects();

                return _projectList;
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

        public bool ReadOnlyMode
        {
            get { return !EditMode; }
        }

        public DelegateCommand RemoveFileCommand
        {
            get { return _removeFile; }
        }

        public DelegateCommand<Test> RemoveTestCommand
        {
            get { return _removeTest; }
        }

        public DelegateCommand SaveCommand
        {
            get { return _save; }
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

        public ReportFile SelectedFile
        {
            get { return _selectedFile; }
            set
            {
                _selectedFile = value;
                _openFile.RaiseCanExecuteChanged();
                _removeFile.RaiseCanExecuteChanged();

                RaisePropertyChanged("SelectedFile");
            }
        } 

        public DelegateCommand StartEditCommand
        {
            get { return _startEdit; }
        }

        public IEnumerable<TestWrapper> TestList
        {
            get { return _testList; }
            private set
            {
                _testList = value;
                RaisePropertyChanged("TestList");
            }
        }

        public double TotalDuration
        {
            get
            {
                return (_instance == null) ? 0 : _instance.TotalDuration;
            }
        }
    }
}
