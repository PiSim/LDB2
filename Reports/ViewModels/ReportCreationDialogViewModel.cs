using DBManager;
using DBManager.Services;
using Infrastructure;
using Infrastructure.Events;
using Infrastructure.Wrappers;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;

namespace Reports.ViewModels
{   
    public class ReportCreationDialogViewModel : BindableBase
    {
        private ControlPlan _selectedControlPlan;
        private DBPrincipal _principal;
        private DelegateCommand<Window> _cancel, _confirm;
        private ICollection<ISelectableRequirement> _requirementList;
        private IEnumerable<Person> _techList;
        private Int32 _number;
        private Person _author;
        private Report _reportInstance;
        private Specification _selectedSpecification;
        private SpecificationVersion _selectedVersion;
        private string _batchNumber, _category;
        
        public ReportCreationDialogViewModel(DBPrincipal principal) : base()
        {
            _principal = principal;

            _techList = PeopleService.GetPeople(PersonRoleNames.MaterialTestingTech);
            _author = _techList.First(prs => prs.ID == _principal.CurrentPerson.ID);
            _requirementList = new List<ISelectableRequirement>();

            _confirm = new DelegateCommand<Window>(
                parent => {
                    _reportInstance = new Report();
                    _reportInstance.Author = _author;
                    _reportInstance.Batch = MaterialService.GetBatch(_batchNumber);
                    _reportInstance.Category = "TR";
                    if (_selectedControlPlan != null)
                        _reportInstance.Description = _selectedControlPlan.Name;
                    else
                        _reportInstance.Description = _selectedSpecification.Description;
                    _reportInstance.IsComplete = false;
                    _reportInstance.Number = _number;
                    _reportInstance.SpecificationVersion = _selectedVersion;
                    _reportInstance.StartDate = DateTime.Now.ToShortDateString();
                    
                    _reportInstance.Tests =  new List<Test>(ReportServiceProvider.GenerateTestList(_requirementList));

                    _reportInstance.Create();
                    
                    parent.DialogResult = true;
                },
                parent => IsValidInput);
                
            _cancel = new DelegateCommand<Window>(
                parent => {
                    parent.DialogResult = false;    
                });
        }
        
        public Person Author
        {
            get { return _author; }
            set 
            { 
                _author = value; 
                RaisePropertyChanged("Author");
            }
        }

        public string BatchNumber
        {
            get { return _batchNumber; }
            set
            {
                _batchNumber = value;
                RaisePropertyChanged("BatchNumber");
            }
        }
        
        public DelegateCommand<Window> CancelCommand
        {
            get { return _cancel; }
        }

        public string Category
        {
            get { return _category; }
            set { _category = value; }
        }
        
        public DelegateCommand<Window> ConfirmCommand
        {
            get { return _confirm; }
        }

        public IEnumerable<ControlPlan> ControlPlanList
        {
            get
            {
                if (_selectedSpecification == null)
                    return new List<ControlPlan>();
                else
                    return _selectedSpecification.ControlPlans;
            }
        }
        
        public bool IsValidInput
        {
            get { return true; }
        }
        
        public Int32 Number
        {
            get { return _number; }
            set { _number = value; }
        }

        public IEnumerable<ISelectableRequirement> RequirementList
        {
            get { return _requirementList; }
        }

        public Report ReportInstance
        {
            get { return _reportInstance; }
        }

        public ControlPlan SelectedControlPlan
        {
            get { return _selectedControlPlan; }
            set
            {
                _selectedControlPlan = value;
                RaisePropertyChanged("SelectedControlPlan");
                if (value != null)
                {
                    CommonServices.ApplyControlPlan(_requirementList, _selectedControlPlan);
                }
            }
        }

        public Specification SelectedSpecification
        {
            get { return _selectedSpecification; }
            set
            {
                _selectedSpecification = value;
                _selectedSpecification.Load();
                
                RaisePropertyChanged("VersionList");
                RaisePropertyChanged("ControlPlanList");

                SelectedControlPlan = ControlPlanList.FirstOrDefault(cp => cp.IsDefault);
                SelectedVersion = VersionList.FirstOrDefault(sv => sv.IsMain);
            }
        }

        public SpecificationVersion SelectedVersion
        {
            get { return _selectedVersion; }
            set 
            { 
                _selectedVersion = value;
                
                if (_selectedVersion != null)
                    _requirementList = new List<ISelectableRequirement>(ReportService.GenerateRequirementList(_selectedVersion)
                                                                                    .Select(req => new ReportItemWrapper(req)));

                else
                    _requirementList = new List<ISelectableRequirement>();

                RaisePropertyChanged("RequirementList");
                RaisePropertyChanged("SelectedVersion");
            }
        }
        
        public IEnumerable<Specification> SpecificationList
        {
            get { return SpecificationService.GetSpecifications(); }
        }
        
        public IEnumerable<SpecificationVersion> VersionList
        {
            get
            {
                if (_selectedSpecification == null)
                    return new List<SpecificationVersion>();

                else
                    return _selectedSpecification.SpecificationVersions;
            }
        }

        public IEnumerable<Person> TechList
        {
            get
            {
                return PeopleService.GetPeople(PersonRoleNames.MaterialTestingTech);
            }
        }
    }
}