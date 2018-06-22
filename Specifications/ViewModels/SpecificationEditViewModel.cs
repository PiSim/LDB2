using DBManager;
using DBManager.EntityExtensions;
using DBManager.Services;
using Infrastructure;
using Infrastructure.Events;
using Infrastructure.Wrappers;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace Specifications.ViewModels
{
    public class SpecificationEditViewModel : BindableBase, INotifyDataErrorInfo
    {
        private bool _editMode;
        private ControlPlan _selectedControlPlan;
        private DBPrincipal _principal;
        private DelegateCommand _addControlPlan, 
                                _addFile,
                                _addVersion,
                                _closeAddMethodView,
                                _openFile,
                                _openReport, 
                                _removeControlPlan, 
                                _removeFile,
                                _removeVersion,
                                _save,
                                _startEdit;
        private DelegateCommand<Method> _addTest;
        private readonly Dictionary<string, ICollection<string>> _validationErrors = new Dictionary<string, ICollection<string>>();
        private EventAggregator _eventAggregator;
        private readonly IDataService _dataService;
        private IEnumerable<Method> _methodList;
        private IReportService _reportService;
        private Report _selectedReport;
        private Specification _instance;
        private SpecificationVersion _selectedVersion;
        private StandardFile _selectedFile;

        public SpecificationEditViewModel(DBPrincipal principal,
                                            EventAggregator aggregator,
                                            IDataService dataService,
                                            IReportService reportService) 
            : base()
        {
            _dataService = dataService;
            _eventAggregator = aggregator;
            _principal = principal;
            _reportService = reportService;


            _addControlPlan = new DelegateCommand(
                () =>
                {
                    ControlPlan temp = _instance.AddControlPlan();

                    RaisePropertyChanged("ControlPlanList");
                });
            
            _addFile = new DelegateCommand(
                () =>
                {
                    OpenFileDialog fileDialog = new OpenFileDialog
                    {
                        Multiselect = true
                    };

                    if (fileDialog.ShowDialog() == DialogResult.OK)
                    {
                        foreach (string pth in fileDialog.FileNames)
                        {
                            StandardFile temp = new StandardFile
                            {
                                Path = pth,
                                Description = "",
                                StandardID = _instance.StandardID
                            };

                            temp.Create();
                        }

                        RaisePropertyChanged("FileList");
                    }
                },
                () => _principal.IsInRole(UserRoleNames.SpecificationEdit));

            _addTest = new DelegateCommand<Method>(
                mtd =>
                {
                    Requirement newReq = _reportService.GenerateRequirement(mtd);
                    _instance.AddMethod(newReq);

                    _eventAggregator.GetEvent<SpecificationMethodListChanged>()
                                    .Publish(_instance);
                },
                mtd => CanEdit);

            _addVersion = new DelegateCommand(
                () =>
                {
                    SpecificationVersion temp = new SpecificationVersion
                    {
                        IsMain = false,
                        Name = "Nuova versione",
                        SpecificationID = _instance.ID
                    };

                    temp.Create();

                    RaisePropertyChanged("VersionList");
                },
                () => CanEdit);

            _closeAddMethodView = new DelegateCommand(
                () =>
                {
                    NavigationToken token = new NavigationToken(SpecificationViewNames.SpecificationVersionList,
                                                                null,
                                                                RegionNames.SpecificationVersionTestListEditRegion);

                    _eventAggregator.GetEvent<NavigationRequested>()
                                    .Publish(token);
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

            _openReport = new DelegateCommand(
                () =>
                {
                    NavigationToken token = new NavigationToken(Reports.ViewNames.ReportEditView,
                                                                SelectedReport);
                    _eventAggregator.GetEvent<NavigationRequested>().Publish(token);
                },
                () => _selectedReport != null);

            _removeControlPlan = new DelegateCommand(
                () =>
                {
                    _selectedControlPlan.Delete();
                    RaisePropertyChanged("ControlPlanList");
                    SelectedControlPlan = null;
                }, 
                () => CanEdit 
                    && _selectedControlPlan != null
                    && !_selectedControlPlan.IsDefault);
            
            _removeFile = new DelegateCommand(
                () =>
                {
                    _selectedFile.Delete();
                    SelectedFile = null;
                },
                () => _principal.IsInRole(UserRoleNames.SpecificationEdit) && _selectedFile != null);
            
            _removeVersion = new DelegateCommand(
                () =>
                {
                    _selectedVersion.Delete();
                    SelectedVersion = null;

                    RaisePropertyChanged("VersionList");
                },
                () => _principal.IsInRole(UserRoleNames.Admin) 
                    && _selectedVersion != null);

            _save = new DelegateCommand(
                () =>
                {
                    _instance.Update();
                    _instance.Standard.Update();
                    EditMode = false;
                },
                () => _editMode && !HasErrors);

            _startEdit = new DelegateCommand(
                () =>
                {
                    EditMode = true;
                },
                () => _principal.IsInRole(UserRoleNames.SpecificationEdit) && !_editMode);

            // Event Subscriptions


            _eventAggregator.GetEvent<MethodChanged>()
                            .Subscribe(
                tkn =>
                {
                    _methodList = null;
                    RaisePropertyChanged("MethodList");
                });

            _eventAggregator.GetEvent<ReportCreated>().Subscribe(
                report => RaisePropertyChanged("ReportList"));

        }

        #region INotifyDataErrorInfo interface elements

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName)
                || !_validationErrors.ContainsKey(propertyName))
                return null;

            return _validationErrors[propertyName];
        }

        public bool HasErrors
        {
            get { return _validationErrors.Count > 0; }
        }

        private void RaiseErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        #endregion

        public DelegateCommand AddControlPlanCommand
        {
            get { return _addControlPlan; }
        }
        
        public DelegateCommand AddFileCommand
        {
            get { return _addFile; }
        }

        public DelegateCommand<Method> AddTestCommand
        {
            get { return _addTest; }
        }

        public DelegateCommand AddVersionCommand
        {
            get { return _addVersion; }
        }

        private bool CanEdit
        {
            get { return _principal.IsInRole(UserRoleNames.SpecificationEdit); }
        }

        public string ControlPlanEditRegionName
        {
            get { return RegionNames.ControlPlanEditRegion; }
        }

        public IEnumerable<ControlPlan> ControlPlanList
        {
            get
            {
                if (_instance == null)
                    return null;

                return _instance.GetControlPlans();
            }
        }

        public string CurrentIssue
        {
            get { return _instance?.Standard.CurrentIssue; }
            set
            {
                _instance.Standard.CurrentIssue = value;
            }
        }

        public string Description
        {
            get
            {
                if (_instance == null)
                    return null;

                return _instance.Description;
            }
            set { _instance.Description = value; }
        }
        
        public bool EditMode
        {
            get { return _editMode; }
            private set
            {
                _editMode = value;
                RaisePropertyChanged("EditMode");

                _save.RaiseCanExecuteChanged();
                _startEdit.RaiseCanExecuteChanged();
            }
        }

        public IEnumerable<StandardFile> FileList
        {
            get { return _instance.GetFiles(); }
        }

        public SpecificationVersion MainVersion
        {
            get
            {
                if (_instance == null)
                    return null;

                return _instance.SpecificationVersions.First(ver => ver.IsMain);
            }
        }

        public IEnumerable<Method> MethodList
        {
            get
            {
                if (_methodList == null)
                    _methodList = _dataService.GetMethods();
                return _methodList;
            }
        }

        public DelegateCommand OpenFileCommand
        {
            get { return _openFile; }
        }

        public DelegateCommand OpenReportCommand
        {
            get { return _openReport; }
        }

        public IEnumerable<Property> Properties => _dataService.GetProperties();

        public DelegateCommand RemoveControlPlanCommand
        {
            get { return _removeControlPlan; }
        }

        public DelegateCommand RemoveFileCommand
        {
            get { return _removeFile; }
        }
        
        public IEnumerable<Report> ReportList
        {
            get
            {
                return _instance.GetReports();
            }
        }

        public DelegateCommand CloseAddMethodViewCommand
        {
            get { return _closeAddMethodView; }
        }

        public DelegateCommand SaveCommand
        {
            get { return _save; }
        }

        public ControlPlan SelectedControlPlan
        {
            get { return _selectedControlPlan; }
            set
            {
                _selectedControlPlan = value;

                NavigationToken token = new NavigationToken(SpecificationViewNames.ControlPlanEdit,
                                                            _selectedControlPlan,
                                                            RegionNames.ControlPlanEditRegion);

                _eventAggregator.GetEvent<NavigationRequested>()
                                .Publish(token);

                RaisePropertyChanged("SelectedControlPlan");
                RaisePropertyChanged("ControlPlanItemsList");
                _removeControlPlan.RaiseCanExecuteChanged();
            }
        }

        public StandardFile SelectedFile
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

        public Report SelectedReport
        {
            get { return _selectedReport; }
            set 
            { 
                _selectedReport = value; 
                _openReport.RaiseCanExecuteChanged();
            }
        }

        public SpecificationVersion SelectedVersion
        {
            get { return _selectedVersion; }
            set
            {
                _selectedVersion = value;
                NavigationToken token = new NavigationToken(SpecificationViewNames.SpecificationVersionEdit,
                                                            _selectedVersion,
                                                            RegionNames.SpecificationVersionEditRegion);

                _eventAggregator.GetEvent<NavigationRequested>()
                                .Publish(token);
                RaisePropertyChanged("SelectedVersion");
            }
        }

        public Specification SpecificationInstance
        {
            get { return _instance; }
            set
            {
                _instance = value;
                _instance.Load();

                SelectedControlPlan = null;
                SelectedVersion = _instance.SpecificationVersions.FirstOrDefault(spcv => spcv.IsMain);

                EditMode = false;

                NavigationToken token = new NavigationToken(SpecificationViewNames.SpecificationVersionList,
                                                            null,
                                                            RegionNames.SpecificationVersionTestListEditRegion);
                _eventAggregator.GetEvent<NavigationRequested>()
                                .Publish(token);

                RaisePropertyChanged("ControlPlanList");
                RaisePropertyChanged("CurrentIssue");
                RaisePropertyChanged("Description");
                RaisePropertyChanged("FileList");
                RaisePropertyChanged("MainVersion");
                RaisePropertyChanged("MainVersionRequirements");
                RaisePropertyChanged("ReportList");
                RaisePropertyChanged("SpecificationName");
                RaisePropertyChanged("Standard");
                RaisePropertyChanged("VersionList");
            }
        }

        public string SpecificationName
        {
            get => _instance?.Name;
            set => _instance.Name = value;
        }

        public string SpecificationVersionEditRegionName
        {
            get { return RegionNames.SpecificationVersionEditRegion; }
        }

        public string SpecificationVersionTestListEditRegionName
        {
            get { return RegionNames.SpecificationVersionTestListEditRegion; }
        }

        public string SpecificationEditFileRegionName
        {
            get { return RegionNames.SpecificationEditFileRegion; }
        }

        public string Standard
        {
            get
            {
                if (_instance == null)
                    return null;

                return _instance.Standard.Name;
            }

            set
            {
                _instance.Standard.Name = value;
            }
        }

        public DelegateCommand StartEditCommand
        {
            get { return _startEdit; }
        }

        public IEnumerable<SpecificationVersion> VersionList
        {
            get
            {
                return _instance.GetVersions();
            }
        }
    }
}
