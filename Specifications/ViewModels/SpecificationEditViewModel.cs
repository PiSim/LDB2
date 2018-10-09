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
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Specifications.ViewModels
{
    public class SpecificationEditViewModel : BindableBase, INotifyDataErrorInfo
    {
        #region Fields

        private readonly IDataService<LabDbEntities> _labDbData;
        private readonly Dictionary<string, ICollection<string>> _validationErrors = new Dictionary<string, ICollection<string>>();
        private bool _editMode;
        private IEventAggregator _eventAggregator;
        private Specification _instance;
        private IEnumerable<MethodVariant> _methodVariantList;
        private IReportService _reportService;
        private ControlPlan _selectedControlPlan;
        private StandardFile _selectedFile;
        private Report _selectedReport;
        private SpecificationVersion _selectedVersion;

        #endregion Fields

        #region Constructors

        public SpecificationEditViewModel(IDataService<LabDbEntities> labDbData,
                                            IEventAggregator aggregator,
                                            IReportService reportService)
        {
            _labDbData = labDbData;
            _eventAggregator = aggregator;
            _reportService = reportService;

            AddControlPlanCommand = new DelegateCommand(
                () =>
                {
                    ControlPlan temp = _instance.AddControlPlan();

                    RaisePropertyChanged("ControlPlanList");
                });

            AddFileCommand = new DelegateCommand(
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

                            _labDbData.Execute(new InsertEntityCommand(temp));
                        }

                        RaisePropertyChanged("FileList");
                    }
                },
                () => Thread.CurrentPrincipal.IsInRole(UserRoleNames.SpecificationEdit));

            AddTestCommand = new DelegateCommand<MethodVariant>(
                mtd =>
                {
                    Requirement newReq = _reportService.GenerateRequirement(mtd);
                    _instance.AddMethod(newReq);

                    _eventAggregator.GetEvent<SpecificationMethodListChanged>()
                                    .Publish(_instance);
                },
                mtd => CanEdit);

            AddVersionCommand = new DelegateCommand(
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

            CloseAddMethodViewCommand = new DelegateCommand(
                () =>
                {
                    NavigationToken token = new NavigationToken(SpecificationViewNames.SpecificationVersionList,
                                                                null,
                                                                RegionNames.SpecificationVersionTestListEditRegion);

                    _eventAggregator.GetEvent<NavigationRequested>()
                                    .Publish(token);
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

            OpenReportCommand = new DelegateCommand(
                () =>
                {
                    NavigationToken token = new NavigationToken(Reports.ViewNames.ReportEditView,
                                                                SelectedReport);
                    _eventAggregator.GetEvent<NavigationRequested>().Publish(token);
                },
                () => _selectedReport != null);

            RemoveControlPlanCommand = new DelegateCommand(
                () =>
                {
                    _selectedControlPlan.Delete();
                    RaisePropertyChanged("ControlPlanList");
                    SelectedControlPlan = null;
                },
                () => CanEdit
                    && _selectedControlPlan != null
                    && !_selectedControlPlan.IsDefault);

            RemoveFileCommand = new DelegateCommand(
                () =>
                {
                    _labDbData.Execute(new DeleteEntityCommand(_selectedFile));
                    SelectedFile = null;
                },
                () => Thread.CurrentPrincipal.IsInRole(UserRoleNames.SpecificationEdit) && _selectedFile != null);

            SaveCommand = new DelegateCommand(
                () =>
                {
                    _labDbData.Execute(new UpdateEntityCommand(_instance));
                    _labDbData.Execute(new UpdateEntityCommand(_instance.Standard));
                    EditMode = false;
                },
                () => _editMode && !HasErrors);

            StartEditCommand = new DelegateCommand(
                () =>
                {
                    EditMode = true;
                },
                () => Thread.CurrentPrincipal.IsInRole(UserRoleNames.SpecificationEdit) && !_editMode);

            // Event Subscriptions

            _eventAggregator.GetEvent<MethodChanged>()
                            .Subscribe(
                tkn =>
                {
                    _methodVariantList = null;
                    RaisePropertyChanged("MethodVariantList");
                });

            _eventAggregator.GetEvent<ReportCreated>().Subscribe(
                report => RaisePropertyChanged("ReportList"));
        }

        #endregion Constructors

        #region INotifyDataErrorInfo interface elements

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public bool HasErrors => _validationErrors.Count > 0;

        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName)
                || !_validationErrors.ContainsKey(propertyName))
                return null;

            return _validationErrors[propertyName];
        }

        private void RaiseErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
        }

        #endregion INotifyDataErrorInfo interface elements

        #region Properties

        public DelegateCommand AddControlPlanCommand { get; }

        public DelegateCommand AddFileCommand { get; }

        public DelegateCommand<MethodVariant> AddTestCommand { get; }

        public DelegateCommand AddVersionCommand { get; }

        public DelegateCommand CloseAddMethodViewCommand { get; }

        public string ControlPlanEditRegionName => RegionNames.ControlPlanEditRegion;

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

                SaveCommand.RaiseCanExecuteChanged();
                StartEditCommand.RaiseCanExecuteChanged();
            }
        }

        public IEnumerable<StandardFile> FileList => _instance.GetFiles();

        public SpecificationVersion MainVersion
        {
            get
            {
                if (_instance == null)
                    return null;

                return _instance.SpecificationVersions.First(ver => ver.IsMain);
            }
        }

        public IEnumerable<MethodVariant> MethodVariantList
        {
            get
            {
                if (_methodVariantList == null)
                    _methodVariantList = _labDbData.RunQuery(new MethodVariantsQuery()).ToList();
                return _methodVariantList;
            }
        }

        public DelegateCommand OpenFileCommand { get; }

        public DelegateCommand OpenReportCommand { get; }

        public IEnumerable<Property> Properties => _labDbData.RunQuery(new PropertiesQuery()).ToList();

        public DelegateCommand RemoveControlPlanCommand { get; }

        public DelegateCommand RemoveFileCommand { get; }

        public IEnumerable<Report> ReportList => _instance.SpecificationVersions.SelectMany(spcver => spcver.Reports).ToList();

        public DelegateCommand SaveCommand { get; }

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
                RemoveControlPlanCommand.RaiseCanExecuteChanged();
            }
        }

        public StandardFile SelectedFile
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

        public Report SelectedReport
        {
            get { return _selectedReport; }
            set
            {
                _selectedReport = value;
                OpenReportCommand.RaiseCanExecuteChanged();
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

        public string SpecificationEditFileRegionName => RegionNames.SpecificationEditFileRegion;

        public Specification SpecificationInstance
        {
            get { return _instance; }
            set
            {
                _instance = value;

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

        public string SpecificationVersionEditRegionName => RegionNames.SpecificationVersionEditRegion;

        public string SpecificationVersionTestListEditRegionName => RegionNames.SpecificationVersionTestListEditRegion;

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

        public DelegateCommand StartEditCommand { get; }

        public IEnumerable<SpecificationVersion> VersionList => _instance.GetVersions();

        private bool CanEdit => Thread.CurrentPrincipal.IsInRole(UserRoleNames.SpecificationEdit);

        #endregion Properties
    }
}