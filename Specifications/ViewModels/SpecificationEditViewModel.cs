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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace Specifications.ViewModels
{
    public class SpecificationEditViewModel : BindableBase, INotifyDataErrorInfo
    {
        private bool _editMode,
                    _testListEditMode;
        private ControlPlan _selectedControlPlan;
        private DBPrincipal _principal;
        private DelegateCommand _addControlPlan, 
                                _addFile,
                                _addTest, 
                                _addVersion,  
                                _openFile,
                                _openReport, 
                                _removeControlPlan, 
                                _removeFile,
                                _removeTest, 
                                _removeVersion,
                                _returnToVersionList,
                                _save,
                                _startEdit,
                                _startTestListEdit;
        private readonly Dictionary<string, ICollection<string>> _validationErrors = new Dictionary<string, ICollection<string>>();
        private EventAggregator _eventAggregator;
        private List<ControlPlanItemWrapper> _controlPlanItemsList;
        private Method _selectedToAdd;
        private Property _filterProperty;
        private Requirement _selectedToRemove;
        private Report _selectedReport;
        private Specification _instance;
        private SpecificationVersion _selectedVersion;
        private StandardFile _selectedFile;

        public SpecificationEditViewModel(DBPrincipal principal,
                                            EventAggregator aggregator) 
            : base()
        {
            _eventAggregator = aggregator;
            _principal = principal;

            _addControlPlan = new DelegateCommand(
                () =>
                {
                    ControlPlan temp = new ControlPlan();
                    temp.Name = "Nuovo Piano di Controllo";
                    temp.Specification = _instance;
                    temp.Create();

                    SelectedControlPlan = temp;
                    RaisePropertyChanged("ControlPlanList");
                });
            
            _addFile = new DelegateCommand(
                () =>
                {
                    OpenFileDialog fileDialog = new OpenFileDialog();
                    fileDialog.Multiselect = true;

                    if (fileDialog.ShowDialog() == DialogResult.OK)
                    {
                        foreach (string pth in fileDialog.FileNames)
                        {
                            StandardFile temp = new StandardFile();
                            temp.Path = pth;
                            temp.Description = "";
                            temp.standardID = _instance.StandardID;

                            temp.Create();
                        }

                        RaisePropertyChanged("FileList");
                    }
                },
                () => _principal.IsInRole(UserRoleNames.SpecificationEdit));

            _addTest = new DelegateCommand(
                () =>
                {
                    Requirement newReq = CommonProcedures.GenerateRequirement(_selectedToAdd);
                    _instance.AddMethod(newReq);
                    RaisePropertyChanged("MainVersionRequirements");

                    _eventAggregator.GetEvent<SpecificationMethodListChanged>()
                                    .Publish(_instance);
                },
                () => CanEdit
                    && _selectedToAdd != null);

            _addVersion = new DelegateCommand(
                () =>
                {
                    SpecificationVersion temp = new SpecificationVersion();
                    temp.IsMain = false;
                    temp.Name = "Nuova versione";
                    temp.SpecificationID = _instance.ID;

                    temp.Create();

                    RaisePropertyChanged("VersionList");
                },
                () => CanEdit);

            _openFile = new DelegateCommand(
                () =>
                {
                    System.Diagnostics.Process.Start(_selectedFile.Path);
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
                    _instance.ControlPlans.Remove(_selectedControlPlan);
                    SelectedControlPlan = null;
                }, 
                () => CanEdit 
                    && _selectedControlPlan != null);
            
            _removeFile = new DelegateCommand(
                () =>
                {
                    _selectedFile.Delete();
                    SelectedFile = null;
                },
                () => _principal.IsInRole(UserRoleNames.SpecificationEdit) && _selectedFile != null);

            _removeTest = new DelegateCommand(
                () =>
                {
                    _selectedToRemove.Delete();
                    RaisePropertyChanged("MainVersionRequirements");

                    _eventAggregator.GetEvent<SpecificationMethodListChanged>()
                                    .Publish(_instance);
                },

                () => CanEdit 
                    && _selectedToRemove != null);

            _removeVersion = new DelegateCommand(
                () =>
                {
                    _selectedVersion.Delete();
                    SelectedVersion = null;

                    RaisePropertyChanged("VersionList");
                },
                () => _principal.IsInRole(UserRoleNames.Admin) 
                    && _selectedVersion != null);

            _returnToVersionList = new DelegateCommand(
                () =>
                {
                    _testListEditMode = false;

                    NavigationToken token = new NavigationToken(SpecificationViewNames.SpecificationVersionList,
                                                                null,
                                                                RegionNames.SpecificationVersionTestListEditRegion);

                    _eventAggregator.GetEvent<NavigationRequested>()
                                    .Publish(token);
                });

            _save = new DelegateCommand(
                () =>
                {
                    _instance.Update();
                    EditMode = false;
                },
                () => _editMode && !HasErrors);

            _startEdit = new DelegateCommand(
                () =>
                {
                    EditMode = true;
                },
                () => _principal.IsInRole(UserRoleNames.Admin) && !_editMode);

            _startTestListEdit = new DelegateCommand(
                () =>
                {
                    _testListEditMode = true;

                    NavigationToken token = new NavigationToken(SpecificationViewNames.SpecificationTestListEdit,
                                                                null,
                                                                RegionNames.SpecificationVersionTestListEditRegion);

                    _eventAggregator.GetEvent<NavigationRequested>()
                                    .Publish(token);
                });

            // Event Subscriptions
            
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

        public DelegateCommand AddTestCommand
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

        public bool CanModifyControlPlan
        {
            get
            {
                if (!_principal.IsInRole(UserRoleNames.SpecificationEdit))
                    return false;

                else if (_selectedControlPlan == null || _selectedControlPlan.IsDefault)
                    return false;

                else
                    return true;
            }
        }

        public List<ControlPlanItemWrapper> ControlPlanItemsList
        {
            get { return _controlPlanItemsList; }
        }

        public IEnumerable<ControlPlan> ControlPlanList
        {
            get
            {
                if (_instance == null)
                    return null;

                return _instance.ControlPlans;
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

        public Property FilterProperty
        {
            get { return _filterProperty; }
            set
            {
                _filterProperty = value;
                RaisePropertyChanged("FilteredMethods");
            }
        }

        public IEnumerable<Method> FilteredMethods
        {
            get
            {
                return SpecificationService.GetMethods().Where(mtd => _filterProperty == null || mtd.Property.ID == _filterProperty.ID);
            }
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

        public IEnumerable<Requirement> MainVersionRequirements
        {
            get
            {
                if (_instance == null)
                    return null;
                return _instance.GetMainVersionRequirements();
            }
        }

        public DelegateCommand OpenFileCommand
        {
            get { return _openFile; }
        }

        public IEnumerable<Property> Properties
        {
            get { return SpecificationService.GetProperties(); }
        }

        public DelegateCommand RemoveControlPlanCommand
        {
            get { return _removeControlPlan; }
        }

        public DelegateCommand RemoveFileCommand
        {
            get { return _removeFile; }
        }

        public DelegateCommand  RemoveTestCommand
        {
            get { return _removeTest; }
        }

        public IEnumerable<Report> ReportList
        {
            get
            {
                return _instance.GetReports();
            }
        }

        public DelegateCommand ReturnToVersionListCommand
        {
            get { return _returnToVersionList; }
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

                if (value == null)
                    _controlPlanItemsList = new List<ControlPlanItemWrapper>();
                
                else
                {

                    _selectedControlPlan.Load();
                    _controlPlanItemsList = new List<ControlPlanItemWrapper>(_instance.SpecificationVersions
                                                                                        .First(sve => sve.IsMain)
                                                                                        .Requirements
                                                                                        .Select(req => new ControlPlanItemWrapper(_selectedControlPlan, req)));
                }

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

        public Method SelectedToAdd
        {
            get { return _selectedToAdd; }
            set
            {
                _selectedToAdd = value;
                _addTest.RaiseCanExecuteChanged();
            }
        }

        public Requirement SelectedToRemove
        {
            get { return _selectedToRemove; }
            set
            {
                _selectedToRemove = value;
                _removeTest.RaiseCanExecuteChanged();
                RaisePropertyChanged("SelectedToRemove");
            }
        }

        public Specification SpecificationInstance
        {
            get { return _instance; }
            set
            {
                _instance = value;

                if (_instance != null)
                {
                    _instance.Load();
                    MainVersion.Load();
                }

                SelectedControlPlan = null;
                SelectedVersion = _instance.SpecificationVersions.FirstOrDefault(spcv => spcv.IsMain);

                EditMode = false;
                _testListEditMode = false;

                NavigationToken token = new NavigationToken(SpecificationViewNames.SpecificationVersionList,
                                                            null,
                                                            RegionNames.SpecificationVersionTestListEditRegion);
                _eventAggregator.GetEvent<NavigationRequested>()
                                .Publish(token);

                RaisePropertyChanged("ControlPlanList");
                RaisePropertyChanged("CurrentIssue");
                RaisePropertyChanged("MainVersion");
                RaisePropertyChanged("MainVersionRequirements");
                RaisePropertyChanged("ReportList");
                RaisePropertyChanged("Standard");
                RaisePropertyChanged("VersionList");
            }
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

        public DelegateCommand StartTestListEditCommand
        {
            get { return _startTestListEdit; }
        }

        public IEnumerable<SpecificationVersion> VersionList
        {
            get
            {
                if (_instance == null)
                    return null;

                return _instance.SpecificationVersions;
            }
        }
    }
}
