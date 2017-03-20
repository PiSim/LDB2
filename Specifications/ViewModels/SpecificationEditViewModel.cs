using DBManager;
using Infrastructure;
using Infrastructure.Events;
using Infrastructure.Tokens;
using Infrastructure.Wrappers;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Specifications.ViewModels
{
    internal class SpecificationEditViewModel : BindableBase
    {
        private ControlPlan _selectedControlPlan;
        private DBEntities _entities;
        private DelegateCommand _addControlPlan, _addFile, _addIssue,
            _addTest, _addVersion, _newReport, _openFile, _openReport, _removeControlPlan, _removeFile, _removeIssue, _removeTest, _removeVersion, _setCurrent;
        private EventAggregator _eventAggregator;
        private List<ControlPlanItemWrapper> _controlPlanItemsList;
        private Method _selectedToAdd;
        private List<RequirementWrapper> _requirementList;
        private ObservableCollection<SpecificationVersion> _versionList;
        private ObservableCollection<StandardIssue> _issueList;
        private Property _filterProperty;
        private Report _selectedReport;
        private Requirement _selectedToRemove;
        private Specification _instance;
        private SpecificationVersion _selectedVersion;
        private StandardFile _selectedFile;
        private StandardIssue _selectedIssue;

        public SpecificationEditViewModel(DBEntities entities,
                                            EventAggregator aggregator) 
            : base()
        {
            _entities = entities;
            _eventAggregator = aggregator;

            _eventAggregator.GetEvent<CommitRequested>().Subscribe(
                () =>
                {
                    _entities.SaveChanges();
                });

            _addControlPlan = new DelegateCommand(
                () =>
                {
                    ControlPlan temp = new ControlPlan();
                    temp.Name = "Nuovo Piano di Controllo";
                    _instance.ControlPlans.Add(temp);
                    SelectedControlPlan = temp;
                    OnPropertyChanged("ControlPlanList");
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
                            _selectedIssue.StandardFiles.Add(temp);
                        }

                        OnPropertyChanged("FileList");
                    }
                });

            _addIssue = new DelegateCommand(
                () =>
                {
                    StandardIssue temp = new StandardIssue();
                    temp.IsCurrent = false;
                    temp.Issue = DateTime.Now.ToShortDateString();
                    temp.Standard = _instance.Standard;

                    _instance.Standard.StandardIssues.Add(temp);

                    _issueList.Add(temp);

                    SelectedIssue = temp;
                });

            _addTest = new DelegateCommand(
                () =>
                {
                    _entities.AddTest(_instance, _selectedToAdd);
                    if (_requirementList != null)
                        
                    OnPropertyChanged("MainVersionRequirements");
                },
                () => _selectedToAdd != null);

            _addVersion = new DelegateCommand(
                () =>
                {
                    SpecificationVersion temp = new SpecificationVersion();
                    temp.IsMain = false;
                    temp.Name = "Nuova versione";

                    _instance.SpecificationVersions.Add(temp);
                    _versionList.Add(temp);
                });
            
            _newReport = new DelegateCommand(
                () =>
                {
                    NewReportToken token = new NewReportToken();
                    _eventAggregator.GetEvent<ReportCreationRequested>().Publish(token);
                });
                
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
                () => _selectedControlPlan != null);

            _removeFile = new DelegateCommand(
                () =>
                {
                    _selectedIssue.StandardFiles.Remove(_selectedFile);
                    SelectedFile = null;
                },
                () => _selectedFile != null );

            _removeIssue = new DelegateCommand(
                () =>
                {
                    _entities.StandardIssues.Remove(_selectedIssue);
                    _issueList.Remove(_selectedIssue);
                    SelectedIssue = null;
                },
                () => _selectedIssue != null);

            _removeTest = new DelegateCommand(
                () =>
                {
                    _entities.Requirements.Remove(_selectedToRemove);
                    OnPropertyChanged("MainVersionRequirements");
                    OnPropertyChanged("RequirementList");
                },

                () => _selectedToRemove != null);

            _setCurrent = new DelegateCommand(
                () =>
                {
                    _instance.Standard.CurrentIssue = _selectedIssue;
                    foreach (StandardIssue stdi in _instance.Standard.StandardIssues)
                    {
                        if (stdi == _selectedIssue)
                            stdi.IsCurrent = true;
                        else
                            stdi.IsCurrent = false;
                    }
                },
                () => _selectedIssue != null);

            // Event Subscriptions

            _eventAggregator.GetEvent<ReportCreated>().Subscribe(
                rpt =>
                {
                    if (rpt.SpecificationVersion.SpecificationID == _instance.ID)
                        OnPropertyChanged("ReportList");
                });

        }
        
        private void GenerateRequirementList()
        {
            List<Requirement> tempReqList = _entities.GenerateRequirementList(_selectedVersion);
            _requirementList = new List<RequirementWrapper>();
            foreach (Requirement rr in tempReqList)
                _requirementList.Add(new RequirementWrapper(rr, _selectedVersion, _entities));
        }


        public DelegateCommand AddControlPlanCommand
        {
            get { return _addControlPlan; }
        }

        public DelegateCommand AddFileCommand
        {
            get { return _addFile; }
        }

        public DelegateCommand AddIssueCommand
        {
            get { return _addIssue; }
        }

        public DelegateCommand AddTestCommand
        {
            get { return _addTest; }
        }

        public DelegateCommand AddVersionCommand
        {
            get { return _addVersion; }
        }

        public List<ControlPlanItemWrapper> ControlPlanItemsList
        {
            get { return _controlPlanItemsList; }
        }

        public List<ControlPlan> ControlPlanList
        {
            get
            {
                if (_instance == null)
                    return null;

                return new List<ControlPlan>(_instance.ControlPlans);
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
        
        public List<StandardFile> FileList
        {
            get
            {
                if (SelectedIssue != null)
                    return new List<StandardFile>(SelectedIssue.StandardFiles);

                else
                    return null;
            }
        }

        public Property FilterProperty
        {
            get { return _filterProperty; }
            set
            {
                _filterProperty = value;
                OnPropertyChanged("FilteredMethods");
            }
        }

        public List<Method> FilteredMethods
        {
            get
            {
                if (_filterProperty == null)
                    return new List<Method>(_entities.Methods);
                else
                    return new List<Method>(
                        _entities.Methods.Where(mtd => mtd.Property.ID == FilterProperty.ID));
            }
        }
        
        public ObservableCollection<StandardIssue> IssueList
        {
            get { return _issueList; }
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

        public ObservableCollection<Requirement> MainVersionRequirements
        {
            get
            {
                if (MainVersion == null)
                    return null;
                return new ObservableCollection<Requirement>(MainVersion.Requirements);
            }
        }
        
        public DelegateCommand NewReportCommand
        {
            get { return _newReport; }
        }

        public DelegateCommand OpenFileCommand
        {
            get { return _openFile; }
        }

        public List<Property> Properties
        {
            get { return new List<Property>(_entities.Properties); }
        }

        public DelegateCommand RemoveControlPlanCommand
        {
            get { return _removeControlPlan; }
        }

        public DelegateCommand RemoveFileCommand
        {
            get { return _removeFile; }
        }

        public DelegateCommand RemoveIssueCommand
        {
            get { return _removeIssue; }
        }

        public DelegateCommand  RemoveTestCommand
        {
            get { return _removeTest; }
        }

        public List<Report> ReportList
        {
            get
            {
                if(_instance == null)
                    return null;

                return new List<Report>(_entities.Reports.Where
                    (rep => rep.SpecificationVersion.Specification.ID == _instance.ID));
            }
        }

        public List<RequirementWrapper> RequirementList
        {
            get { return _requirementList; }
        }

        public ControlPlan SelectedControlPlan
        {
            get { return _selectedControlPlan; }
            set
            {
                _selectedControlPlan = value;

                _controlPlanItemsList = new List<ControlPlanItemWrapper>();
                foreach (Requirement rr in _instance.SpecificationVersions.First(sve => sve.IsMain).Requirements)
                    _controlPlanItemsList.Add(new ControlPlanItemWrapper(_selectedControlPlan, rr));

                OnPropertyChanged("SelectedControlPlan");
                OnPropertyChanged("ControlPlanItemsList");
                _removeControlPlan.RaiseCanExecuteChanged();
            }
        }
        
        public StandardFile SelectedFile
        {
            get { return _selectedFile; }
            set 
            { 
                _selectedFile = value;
                OnPropertyChanged("SelectedFile");
                OpenFileCommand.RaiseCanExecuteChanged();
                RemoveFileCommand.RaiseCanExecuteChanged();
            }
        }
        
        public StandardIssue SelectedIssue
        {
            get { return _selectedIssue; }
            set 
            {
                _selectedIssue = value;
                _setCurrent.RaiseCanExecuteChanged();
                _removeIssue.RaiseCanExecuteChanged();
                OnPropertyChanged("SelectedIssue");
                OnPropertyChanged("FileList");
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
                OnPropertyChanged("SelectedVersionIsNotMain");
                if (_selectedVersion != null)
                {
                    GenerateRequirementList();
                    OnPropertyChanged("RequirementList");
                }
            }
        }

        public bool SelectedVersionIsNotMain
        {
            get { return !_selectedVersion.IsMain; }
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
                OnPropertyChanged("SelectedToRemove");
            }
        }

        public Specification SpecificationInstance
        {
            get { return _instance; }
            set
            {
                _instance = _entities.Specifications.FirstOrDefault(spec => spec.ID == value.ID);
                _issueList = new ObservableCollection<StandardIssue>(_instance.Standard.StandardIssues);
                _versionList = new ObservableCollection<SpecificationVersion>(_instance.SpecificationVersions);

                OnPropertyChanged("ControlPlanList");
                OnPropertyChanged("MainVersion");
                OnPropertyChanged("MainVersionRequirements");
                OnPropertyChanged("ReportList");
                OnPropertyChanged("Standard");
                OnPropertyChanged("VersionList");
            }
        }

        public DelegateCommand SetCurrentCommand
        {
            get { return _setCurrent; }
        }

        public string Standard
        {
            get
            {
                if (_instance == null)
                    return null;

                return _instance.Standard.Organization.Name + " " +
                  _instance.Standard.Name;
            }
        }

        public List<SpecificationVersion> VersionList
        {
            get
            {
                if (_instance == null)
                    return null;

                return new List<SpecificationVersion>(_instance.SpecificationVersions);
            }
        }
    }
}
