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
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Forms;


namespace Reports.ViewModels
{
    public class ExternalReportEditViewModel : BindableBase
    {
        private bool _editMode;
        private DBPrincipal _principal;
        private DelegateCommand _addBatch,
                                _addFile,
                                _openBatch,
                                _openFile,
                                _removeBatch,
                                _removeFile,
                                _save,
                                _startEdit;
        private DelegateCommand<Method> _addMethod,
                                        _removeMethod;
        private EventAggregator _eventAggregator;
        private ExternalReport _instance;
        private ExternalReportFile _selectedFile;
        private IDataService _dataService;
        private IEnumerable<Project> _projectList;
        private List<ExternalResultPresenter> _resultList;
        private IMaterialService _materialService;
        private IReportService _reportService;
        private ObservableCollection<DataGridColumn> _resultColumnCollection;
        private string _batchNumber;
        private TestRecord _selectedRecord;

        public ExternalReportEditViewModel(DBPrincipal principal,
                                            EventAggregator aggregator,
                                            IDataService dataService,
                                            IMaterialService materialService,
                                            IReportService reportService) : base()
        {
            _dataService = dataService;
            _editMode = false;
            _eventAggregator = aggregator;
            _materialService = materialService;
            _principal = principal;
            _reportService = reportService;

            _addBatch = new DelegateCommand(
                () =>
                {
                    Batch tempBatch = _materialService.StartBatchSelection();
                    if (tempBatch != null)
                    {
                        _instance.AddBatch(tempBatch);
                        RefreshTestRecords();
                        ResultColumnCollection = _instance.GetResultPresentationColumns();
                    }
                },
                () => _instance != null && EditMode);

            _addFile = new DelegateCommand(
                () =>
                {
                    OpenFileDialog fileDialog = new OpenFileDialog
                    {
                        InitialDirectory = UserSettings.ExternalReportPath,
                        Multiselect = true
                    };

                    if (fileDialog.ShowDialog() == DialogResult.OK)
                    {
                        foreach (string pth in fileDialog.FileNames)
                        {
                            ExternalReportFile temp = new ExternalReportFile();
                            temp.Path = pth;
                            temp.Description = "";
                            temp.ExternalReportID = _instance.ID;

                            temp.Create();
                        }

                        RaisePropertyChanged("ReportFiles");
                    }
                },
                () => EditMode);

            _addMethod = new DelegateCommand<Method>(
                mtd =>
                {
                    _instance.AddTestMethod(mtd);

                    _instance.Methods = _instance.GetMethods();
                    RaisePropertyChanged("MethodList");

                    RefreshTestRecords();
                },
                mtd => EditMode);

            _openBatch = new DelegateCommand(
                () =>
                {
                    NavigationToken token = new NavigationToken(MaterialViewNames.BatchInfoView,
                                                                _selectedRecord.Batch);
                    _eventAggregator.GetEvent<NavigationRequested>().Publish(token);
                },
                () => _selectedRecord != null);

            _openFile = new DelegateCommand(
                () =>
                {
                    System.Diagnostics.Process.Start(_selectedFile.Path);
                },
                () => _selectedFile != null);

            _removeBatch = new DelegateCommand(
                () =>
                {
                    _selectedRecord.Delete();
                    SelectedRecord = null;
                    RefreshTestRecords();
                    ResultColumnCollection = _instance.GetResultPresentationColumns();
                },
                () => _selectedRecord != null && EditMode);

            _removeFile = new DelegateCommand(
                () =>
                {
                    _selectedFile.Delete();
                    SelectedFile = null;
                },
                () => _selectedFile != null && EditMode);

            _removeMethod = new DelegateCommand<Method>(
                mtd =>
                {
                    _instance.RemoveTestMethod(mtd);

                    _instance.Methods = _instance.GetMethods() as ICollection<Method>;
                    RaisePropertyChanged("MethodList");

                    RefreshTestRecords();
                },
                mtd => EditMode);

            _save = new DelegateCommand(
                () =>
                {
                    _instance.Update(true);
                    EditMode = false;

                    EntityChangedToken token = new EntityChangedToken(_instance,
                                                                    EntityChangedToken.EntityChangedAction.Updated);

                    _eventAggregator.GetEvent<ExternalReportChanged>()
                                    .Publish(token);
                },
                () => _editMode);

            _startEdit = new DelegateCommand(
                () =>
                {
                    EditMode = true;
                },
                () => !_editMode && CanModify);
        }

        #region Methods

        private void RefreshTestRecords()
        {
            _instance.TestRecords = _instance.GetTestRecords(true) as ICollection<TestRecord>;

            _resultList = new List<ExternalResultPresenter>();

            foreach (Tuple<Method, IEnumerable<Test>> resTuple in _instance.GetResultCollection())
                _resultList.Add(new ExternalResultPresenter(resTuple.Item1, resTuple.Item2));

            RaisePropertyChanged("ResultCollection");
            RaisePropertyChanged("RecordList");
        }

        #endregion

        public DelegateCommand AddBatchCommand
        {
            get { return _addBatch; }
        }

        public DelegateCommand AddFileCommand
        {
            get { return _addFile; }
        }

        public DelegateCommand<Method> AddMethodCommand => _addMethod;

        public IEnumerable<Method> AvailableMethodList => _dataService.GetMethods();
        
        public string BatchNumber
        {
            get { return _batchNumber; }
            set
            {
                _batchNumber = value;
                RaisePropertyChanged("BatchNumber");
            }
        }
        
        public string Description
        {
            get => _instance?.Description;

            set => _instance.Description = value;
        }

        public bool CanModify => _principal.IsInRole(UserRoleNames.ExternalReportEdit);

        public bool EditMode
        {
            get { return _editMode; }
            set
            {
                _editMode = value;
                RaisePropertyChanged("CanModifyOrder");
                RaisePropertyChanged("EditMode");

                _addBatch.RaiseCanExecuteChanged();
                _addFile.RaiseCanExecuteChanged();
                _addMethod.RaiseCanExecuteChanged();
                _removeBatch.RaiseCanExecuteChanged();
                _removeFile.RaiseCanExecuteChanged();
                _removeMethod.RaiseCanExecuteChanged();
                _save.RaiseCanExecuteChanged();
                _startEdit.RaiseCanExecuteChanged();
            }
        }

        public string ExternalLab
        {
            get
            {
                if (_instance == null)
                    return null;
                return _instance.ExternalLab.Name;
            }
        }

        public string ExternalReportEditAddMethodRegionName => RegionNames.ExternalReportAddMethodRegion;

        public ExternalReport ExternalReportInstance
        {
            get { return _instance; }
            set
            {
                EditMode = false;

                _instance = value;
                _instance.Load();
                                
                if (_instance != null)
                {
                    _instance.Methods = _instance.GetMethods();
                    RefreshTestRecords();
                }

                else
                {
                    ResultCollection = new List<ExternalResultPresenter>();
                    RaisePropertyChanged("RecordList");
                }
                
                ResultColumnCollection = _instance?.GetResultPresentationColumns();

                SelectedRecord = null;
                SelectedFile = null;

                _addBatch.RaiseCanExecuteChanged();
                _removeBatch.RaiseCanExecuteChanged();

                RaisePropertyChanged("AvailableMethodList");
                RaisePropertyChanged("CanModifyOrder");
                RaisePropertyChanged("Description");
                RaisePropertyChanged("ReportFiles");
                RaisePropertyChanged("ExternalLab");
                RaisePropertyChanged("FormattedNumber");
                RaisePropertyChanged("HasOrder");
                RaisePropertyChanged("MethodList");
                RaisePropertyChanged("SamplesSent");
                RaisePropertyChanged("SamplesSentDate");
                RaisePropertyChanged("OrderNumber");
                RaisePropertyChanged("OrderPrice");
                RaisePropertyChanged("SelectedProject");
                RaisePropertyChanged("ReportReceived");
                RaisePropertyChanged("ReportReceivedDate");
                RaisePropertyChanged("RequestDone");
                RaisePropertyChanged("RequestDate");
                RaisePropertyChanged("Samples");
            }
        }

        public string FormattedNumber => _instance?.FormattedNumber;
         
        public bool HasOrder
        {
            get => (_instance == null) ? false : _instance.HasOrder;
            set
            {
                _instance.HasOrder = value;
                RaisePropertyChanged("CanModifyOrder");
            }
        }

        public IEnumerable<Method> MethodList => _instance?.Methods;

        public DelegateCommand OpenBatchCommand
        {
            get { return _openBatch; }
        }

        public DelegateCommand OpenFileCommand
        {
            get { return _openFile; }
        }

        public string OrderNumber
        {
            get => _instance?.OrderNumber;
            set => _instance.OrderNumber = value;
        }

        public double OrderPrice
        {
            get => (_instance == null) ? 0 : _instance.OrderTotal;
            set => _instance.OrderTotal = value;
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

        public Project Project
        {
            get
            {
                if (_instance == null)
                    return null;

                return _instance.Project;
            }
        }

        public IEnumerable<TestRecord> RecordList => _instance?.TestRecords;

        public DelegateCommand RemoveBatchCommand
        {
            get { return _removeBatch; }
        }

        public DelegateCommand RemoveFileCommand
        {
            get { return _removeFile; }
        }

        public DelegateCommand<Method> RemoveMethodCommand => _removeMethod;

        public IEnumerable<ExternalReportFile> ReportFiles
        {
            get { return _instance.GetExternalReportFiles(); }
        }

        public bool ReportReceived
        {
            get
            {
                if (_instance == null)
                    return false;

                else
                    return _instance.ReportReceived;
            }
            set
            {
                _instance.ReportReceived = value;
                if(value)
                    _instance.ReportReceivedDate = DateTime.Now.Date;
                else
                    _instance.RequestDoneDate = null;

                RaisePropertyChanged("ReportReceivedDate");
            }
        }

        public DateTime? ReportReceivedDate => _instance?.ReportReceivedDate;

        public bool RequestDone
        {
            get
            {
                if (_instance == null)
                    return false;

                else
                    return _instance.RequestDone;
            }
            set
            {
                _instance.RequestDone = value;
                if (value)
                    _instance.RequestDoneDate = DateTime.Now.Date;
                else
                    _instance.RequestDoneDate = null;

                RaisePropertyChanged("RequestDate");
            }
        }

        public DateTime? RequestDate => _instance?.RequestDoneDate;
        
        public IEnumerable<ExternalResultPresenter> ResultCollection
        {
            get => _resultList;
            private set
            {
                _resultList = value.ToList();
                RaisePropertyChanged("ResultCollection");
            }
        }
         
        public ObservableCollection<DataGridColumn> ResultColumnCollection
        {
            get => _resultColumnCollection;
            private set
            {
                _resultColumnCollection = value;
                RaisePropertyChanged("ResultColumnCollection");
            }
        }

        public string Samples
        {
            get
            {
                if (_instance == null)
                    return null;

                return _instance.Samples;
            }

            set { _instance.Samples = value; }
        }
        public bool SamplesSent
        {
            get
            {
                if (_instance == null)
                    return false;

                else
                    return _instance.MaterialSent;
            }

            set
            {
                _instance.MaterialSent = value;

                if (value)
                    _instance.MaterialSentDate = DateTime.Now.Date;
                else
                    _instance.MaterialSentDate = null;

                RaisePropertyChanged("SamplesSentDate");
            }
        }

        public DateTime? SamplesSentDate => _instance?.MaterialSentDate;
        
        // Named 'SelectedRecord' for consistency among views
        public TestRecord SelectedRecord
        {
            get { return _selectedRecord; }
            set
            {
                _selectedRecord = value;
                RaisePropertyChanged("SelectedRecord");
                _openBatch.RaiseCanExecuteChanged();
                _removeBatch.RaiseCanExecuteChanged();
            }
        }
        public DelegateCommand SaveCommand => _save;

        public ExternalReportFile SelectedFile
        {
            get { return _selectedFile; }
            set
            {
                _selectedFile = value;
                _openFile.RaiseCanExecuteChanged();
                _removeFile.RaiseCanExecuteChanged();
            }
        }

        public Project SelectedProject
        {
            get => _projectList.FirstOrDefault(prj => prj.ID == _instance?.ProjectID);
            set
            {
                if (_instance != null) _instance.ProjectID = value.ID;
            }
        } 
        public DelegateCommand StartEditCommand => _startEdit;

        public IEnumerable<TestRecord> TestRecordList => _instance?.GetTestRecords();
    }
}

