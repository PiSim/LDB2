using DBManager;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Specifications.ViewModels
{
    internal class SpecificationEditViewModel : BindableBase
    {
        private ControlPlan _selectedControlPlan;
        private DBEntities _entities;
        private DelegateCommand _addControlPlan, _addFile, _addTest, _removeControlPlan, _removeFile, _removeTest;
        private ObservableCollection<RequirementWrapper> _requirementList;
        private Method _selectedToAdd;
        private Property _filterProperty;
        private Requirement _selectedToRemove;
        private Specification _instance;
        private SpecificationVersion _selectedVersion;
        private StandardFile _selectedFile;
        private StandardIssue _selectedIssue;

        internal SpecificationEditViewModel(DBEntities entities,
                                            Specification instance) 
            : base()
        {
            _requirementList = new ObservableCollection<RequirementWrapper>();
            _entities = entities;
            
            _instance = _entities.Specification.FirstOrDefault(spec => spec.ID == instance.ID);
            
            if (_instance == null)
                throw new NullReferenceException(); 

            _addControlPlan = new DelegateCommand(
                () =>
                {
                    ControlPlan temp = new ControlPlan();
                    temp.Name = "Nuovo Piano di Controllo";
                    _instance.ControlPlans.Add(temp);
                    SelectedControlPlan = temp;
                    OnPropertyChanged("ControlPlanList");
                });

            _addTest = new DelegateCommand(
                () =>
                {
                    _entities.AddTest(_instance, _selectedToAdd);
                    OnPropertyChanged("MainVersionRequirements");
                },
                () => _selectedToAdd != null);

            _removeControlPlan = new DelegateCommand(
                () =>
                {
                    _instance.ControlPlans.Remove(_selectedControlPlan);
                    SelectedControlPlan = null;
                }, 
                () => _selectedControlPlan != null);
            
            _removeTest = new DelegateCommand(
                () =>
                {
                    MainVersion.Requirements.Remove(_selectedToRemove);
                    OnPropertyChanged("MainVersionRequirements");
                },

                () => _selectedToRemove != null);
        }

        public DelegateCommand AddTestCommand
        {
            get { return _addTest; }
        }

        public DelegateCommand AddControlPlanCommand
        {
            get { return _addControlPlan; }
        }

        public List<ControlPlan> ControlPlanList
        {
            get { return new List<ControlPlan>(_instance.ControlPlans); }
        }
        
        public List<StandardFile> FileList
        {
            get { return new List<StandardFile>(SelectedIssue.StandardFiles); }
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

        public Specification Instance
        {
            get { return _instance; }
        }
        
        public List<StandardIssue> IssueList
        {
            get { return new List<StandardIssue>(_instance.StandardIssues); }
        }

        public SpecificationVersion MainVersion
        {
            get { return _instance.SpecificationVersions.First(ver => ver.IsMain == 1); }
        }

        public ObservableCollection<Requirement> MainVersionRequirements
        {
            get { return new ObservableCollection<Requirement>(MainVersion.Requirements); }
        }

        public List<Property> Properties
        {
            get { return new List<Property>(_entities.Properties); }
        }

        public DelegateCommand RemoveControlPlanCommand
        {
            get { return _removeControlPlan; }
        }

        public DelegateCommand  RemoveTestCommand
        {
            get { return _removeTest; }
        }

        public List<Report> ReportList
        {
            get
            {
                return new List<Report>(_entities.Reports.Where
                    (rep => rep.SpecificationVersion.Specification.ID == _instance.ID));
            }
        }

        public ObservableCollection<RequirementWrapper> RequirementList
        {
            get { return _requirementList; }
        }

        public ControlPlan SelectedControlPlan
        {
            get { return _selectedControlPlan; }
            set
            {
                _selectedControlPlan = value;
                OnPropertyChanged("SelectedControlPlan");
                _removeControlPlan.RaiseCanExecuteChanged();
            }
        }
        
        public StandardFile SelectedFile
        {
            get { return _selectedFile; }
            set 
            { 
                _selectedFile = value; 
            }
        }
        
        public StandardIssue SelectedIssue
        {
            get { return _selectedIssue; }
            set 
            {
                _selectedIssue = value;
                OnPropertyChanged("FileList");
            }
        }

        public SpecificationVersion SelectedVersion
        {
            get { return _selectedVersion; }
            set
            {
                _selectedVersion = value;
                OnPropertyChanged("SelectedVersionIsNotMain");
                List<Requirement> tempReqList = _entities.GenerateRequirementList(_selectedVersion);
                _requirementList.Clear();
                foreach (Requirement rr in tempReqList)
                    _requirementList.Add(new RequirementWrapper(rr, _selectedVersion, _entities));
            }
        }

        public bool SelectedVersionIsNotMain
        {
            get { return !(_selectedVersion.IsMain == 1); }
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

        public string Standard
        {
            get
            {
                return _instance.Standard.Organization.Name + " " +
                  _instance.Standard.Name;
            }
        }

        public List<SpecificationVersion> VersionList
        {
            get { return new List<SpecificationVersion>(_instance.SpecificationVersions); }
        }
    }
}
