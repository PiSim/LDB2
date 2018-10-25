using Controls.Views;
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
using Reports.Commands;
using Reports.Queries;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace Reports.ViewModels
{
    public class ExternalReportEditViewModel : BindableBase
    {
        #region Fields

        private string _batchNumber;
        private bool _editMode;
        private IEventAggregator _eventAggregator;
        private IEnumerable<TestRecord> _testRecordList;
        private IEnumerable<MethodVariant> _methodVariantList;
        private ExternalReport _instance;
        private IDataService<LabDbEntities> _labDbData;
        private IEnumerable<Project> _projectList;
        private IReportService _reportService;
        private ObservableCollection<DataGridColumn> _resultColumnCollection;
        private List<ExternalResultPresenter> _resultList;
        private ExternalReportFile _selectedFile;
        private TestRecord _selectedRecord;

        #endregion Fields

        #region Constructors

        public ExternalReportEditViewModel(IEventAggregator aggregator,
                                            IDataService<LabDbEntities> labDbData,
                                            IReportService reportService)
        {
            _labDbData = labDbData;
            _editMode = false;
            _eventAggregator = aggregator;
            _reportService = reportService;

            AddBatchCommand = new DelegateCommand(
                () =>
                {
                    Batch tempBatch;

                    BatchPickerDialog batchPicker = new BatchPickerDialog();
                    if (batchPicker.ShowDialog() == true)
                    {
                        tempBatch = _labDbData.RunQuery(new BatchQuery() { Number = batchPicker.BatchNumber });
                        _instance.AddBatch(tempBatch);
                        RefreshTestRecords();
                        ResultColumnCollection = GenerateResultPresentationColumns();
                    }
                },
                () => _instance != null && EditMode);

            AddFileCommand = new DelegateCommand(
                () =>
                {
                    System.Windows.Forms.OpenFileDialog fileDialog = new System.Windows.Forms.OpenFileDialog
                    {
                        InitialDirectory = UserSettings.ExternalReportPath,
                        Multiselect = true
                    };

                    if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        foreach (string pth in fileDialog.FileNames)
                        {
                            ExternalReportFile temp = new ExternalReportFile();
                            temp.Path = pth;
                            temp.Description = "";
                            temp.ExternalReportID = _instance.ID;

                            _labDbData.Execute(new InsertEntityCommand(temp));
                        }

                        RaisePropertyChanged("ReportFiles");
                    }
                },
                () => EditMode);

            AddMethodCommand = new DelegateCommand<MethodVariant>(
                mtd =>
                {
                    _instance.AddTestMethod(mtd);
                    RefreshTestRecords();
                },
                mtd => EditMode);

            OpenBatchCommand = new DelegateCommand(
                () =>
                {
                    NavigationToken token = new NavigationToken(MaterialViewNames.BatchInfoView,
                                                                _selectedRecord.Batch);
                    _eventAggregator.GetEvent<NavigationRequested>().Publish(token);
                },
                () => _selectedRecord != null);

            OpenFileCommand = new DelegateCommand(
                () =>
                {
                    System.Diagnostics.Process.Start(_selectedFile.Path);
                },
                () => _selectedFile != null);

            RemoveBatchCommand = new DelegateCommand(
                () =>
                {
                    _labDbData.Execute(new DeleteEntityCommand(_selectedRecord));
                    SelectedRecord = null;
                    RefreshTestRecords();
                    ResultColumnCollection = GenerateResultPresentationColumns();
                },
                () => _selectedRecord != null && EditMode);

            RemoveFileCommand = new DelegateCommand(
                () =>
                {
                    _labDbData.Execute(new DeleteEntityCommand(_selectedFile));
                    SelectedFile = null;
                },
                () => _selectedFile != null && EditMode);

            RemoveMethodCommand = new DelegateCommand<MethodVariant>(
                mtd =>
                {
                    _instance.RemoveTestMethodVariant(mtd);
                    RefreshTestRecords();
                },
                mtd => EditMode);

            SaveCommand = new DelegateCommand(
                () =>
                {
                    _labDbData.Execute(new UpdateEntityCommand(_instance));
                    _labDbData.Execute(new BulkUpdateSubTestResultsCommand(ResultCollection.SelectMany(erp => erp.TestList.SelectMany(tst => tst.SubTests))));
                    EditMode = false;

                    EntityChangedToken token = new EntityChangedToken(_instance,
                                                                    EntityChangedToken.EntityChangedAction.Updated);

                    _eventAggregator.GetEvent<ExternalReportChanged>()
                                    .Publish(token);
                },
                () => _editMode);

            StartEditCommand = new DelegateCommand(
                () =>
                {
                    EditMode = true;
                },
                () => !_editMode && CanModify);
        }

        #endregion Constructors

        #region Commands

        public DelegateCommand AddBatchCommand { get; }

        public DelegateCommand AddFileCommand { get; }

        public DelegateCommand<MethodVariant> AddMethodCommand { get; }

        public DelegateCommand OpenBatchCommand { get; }

        public DelegateCommand OpenFileCommand { get; }

        public DelegateCommand RemoveBatchCommand { get; }

        public DelegateCommand RemoveFileCommand { get; }

        public DelegateCommand<MethodVariant> RemoveMethodCommand { get; }

        public DelegateCommand SaveCommand { get; }
        public DelegateCommand StartEditCommand { get; }

        #endregion Commands

        #region Methods
        /// <summary>
        /// Generates an ObservableCollection of DataGridColumns
        /// that will be used to correctly visualize the results in a DataGrid.
        /// TODO: Move method to the view. As Multicast converter maybe?
        /// </summary>
        /// <returns>An ObservableCollection of DataGridColumns</returns>
        public ObservableCollection<DataGridColumn> GenerateResultPresentationColumns()
        {
            ObservableCollection<DataGridColumn> output = new ObservableCollection<DataGridColumn>();

            output.Add(new DataGridTextColumn()
            {
                Header = "Metodo",
                Binding = new Binding("MethodName"),
                IsReadOnly = true,
                Width = 100
            });

            output.Add(new DataGridTextColumn()
            {
                Header = "Variante",
                Binding = new Binding("MethodVariantName"),
                IsReadOnly = true,
                Width = 150
            });

            output.Add(new DataGridTemplateColumn()
            {
                Header = "Prove",
                CellTemplate = (DataTemplate)Application.Current.Resources[ResourceKeys.DataGridSubTestNameCellTemplate],
                Width = 150
            });

            output.Add(new DataGridTemplateColumn()
            {
                Header = "UM",
                CellTemplate = (DataTemplate)Application.Current.Resources[ResourceKeys.DataGridSubTestUMCellTemplate],
                Width = 150
            });

            foreach (TestRecord tstr in TestRecordList)
            {
                DataGridColumn resultColumn = new DataGridTemplateColumn()
                {
                    Header = tstr.Batch.Number,
                    CellTemplate = (DataTemplate)Application.Current.Resources[ResourceKeys.DataGridSubTestExternalResultCellTemplate],
                    CellStyle = (Style)Application.Current.Resources[ResourceKeys.DataGridCellEnabledOnEditModeStyle],
                    Width = 150
                };

                DataGridColumnTagExtension.SetTag(resultColumn, tstr.BatchID);

                output.Add(resultColumn);
            }

            return output;
        }

        /// <summary>
        /// Generates a list of test for each methodVariant associated with the Report
        /// generated from the entity collections loaded in the ViewModel
        /// </summary>
        /// <returns>An IEnumerable of Tuples where Value1 is a methodVAriant and
        /// Value2 is an IEnumerable of tests</returns>
        private IEnumerable<Tuple<MethodVariant, IEnumerable<Test>>> GenerateResultCollection()
        {
            List<Tuple<MethodVariant, IEnumerable<Test>>> output = new List<Tuple<MethodVariant, IEnumerable<Test>>>();
            foreach (MethodVariant mtdvar in MethodVariantList)
            {
                IEnumerable<Test> testList = TestRecordList.SelectMany(tstr => tstr.Tests)
                                                        .Where(tst => tst.MethodVariantID == mtdvar.ID);

                output.Add(new Tuple<MethodVariant, IEnumerable<Test>>(mtdvar, testList));
            }

            return output;
        }

        private void RefreshTestRecords()
        {
            //Retrieve up-to-date TestRecords
            TestRecordList = (_instance == null) ? new List<TestRecord>() : _labDbData.RunQuery(new TestRecordsQuery() {ExternalReportID = _instance.ID }).ToList();

            //Retrieve up-to-date Method Variante
            MethodVariantList = (_instance == null) ? new List<MethodVariant>() : _labDbData.RunQuery(new MethodVariantsQuery() { ExternalReportID = _instance.ID, IncludeObsolete = true }).ToList();

            //Generate new Result Collection
            IEnumerable<Tuple<MethodVariant, IEnumerable<Test>>> resultCollection = GenerateResultCollection();
            _resultList = new List<ExternalResultPresenter>(resultCollection.Select(tp => new ExternalResultPresenter(tp.Item1, tp.Item2)));

            RaisePropertyChanged("ResultCollection");
        }

        #endregion Methods

        #region Properties

        public IEnumerable<MethodVariant> AvailableMethodVariantList => _labDbData.RunQuery(new MethodVariantsQuery()).ToList();

        public string BatchNumber
        {
            get { return _batchNumber; }
            set
            {
                _batchNumber = value;
                RaisePropertyChanged("BatchNumber");
            }
        }

        public bool CanModify => Thread.CurrentPrincipal.IsInRole(UserRoleNames.ExternalReportEdit);

        public string Description
        {
            get => _instance?.Description;

            set => _instance.Description = value;
        }

        public bool EditMode
        {
            get { return _editMode; }
            set
            {
                _editMode = value;
                RaisePropertyChanged("CanModifyOrder");
                RaisePropertyChanged("EditMode");

                AddBatchCommand.RaiseCanExecuteChanged();
                AddFileCommand.RaiseCanExecuteChanged();
                AddMethodCommand.RaiseCanExecuteChanged();
                RemoveBatchCommand.RaiseCanExecuteChanged();
                RemoveFileCommand.RaiseCanExecuteChanged();
                RemoveMethodCommand.RaiseCanExecuteChanged();
                SaveCommand.RaiseCanExecuteChanged();
                StartEditCommand.RaiseCanExecuteChanged();
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

                _instance = _labDbData.RunQuery(new ExternalReportQuery(value.ID));

                RefreshTestRecords();

                ResultColumnCollection = GenerateResultPresentationColumns();

                SelectedRecord = null;
                SelectedFile = null;

                AddBatchCommand.RaiseCanExecuteChanged();
                RemoveBatchCommand.RaiseCanExecuteChanged();

                RaisePropertyChanged("AvailableMethodVariantList");
                RaisePropertyChanged("CanModifyOrder");
                RaisePropertyChanged("Description");
                RaisePropertyChanged("ReportFiles");
                RaisePropertyChanged("ExternalLab");
                RaisePropertyChanged("FormattedNumber");
                RaisePropertyChanged("HasOrder");
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

        public IEnumerable<MethodVariant> MethodVariantList
        {
            get => _methodVariantList;
            private set
            {
                _methodVariantList = value;
                RaisePropertyChanged("MethodVariantList");
            }
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

        public Project Project
        {
            get
            {
                if (_instance == null)
                    return null;

                return _instance.Project;
            }
        }

        public IEnumerable<Project> ProjectList
        {
            get
            {
                if (_projectList == null)
                    _projectList = _labDbData.RunQuery(new ProjectsQuery()).ToList();

                return _projectList;
            }
        }
        
        public IEnumerable<ExternalReportFile> ReportFiles => _instance.GetExternalReportFiles();

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
                if (value)
                    _instance.ReportReceivedDate = DateTime.Now.Date;
                else
                    _instance.RequestDoneDate = null;

                RaisePropertyChanged("ReportReceivedDate");
            }
        }

        public DateTime? ReportReceivedDate => _instance?.ReportReceivedDate;

        public DateTime? RequestDate => _instance?.RequestDoneDate;

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

        public ExternalReportFile SelectedFile
        {
            get { return _selectedFile; }
            set
            {
                _selectedFile = value;
                OpenFileCommand.RaiseCanExecuteChanged();
                RemoveFileCommand.RaiseCanExecuteChanged();
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

        // Named 'SelectedRecord' for consistency among views
        public TestRecord SelectedRecord
        {
            get { return _selectedRecord; }
            set
            {
                _selectedRecord = value;
                RaisePropertyChanged("SelectedRecord");
                OpenBatchCommand.RaiseCanExecuteChanged();
                RemoveBatchCommand.RaiseCanExecuteChanged();
            }
        }

        public IEnumerable<TestRecord> TestRecordList
        {
            get => _testRecordList;
            private set
            {
                _testRecordList = value;
                RaisePropertyChanged("TestRecordList");
            }
        }

        #endregion Properties
    }
}