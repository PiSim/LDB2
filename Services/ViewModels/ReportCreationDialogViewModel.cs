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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Services.ViewModels
{   
    public class ReportCreationDialogViewModel : BindableBase
    {
        private Batch _selectedBatch;
        private ControlPlan _selectedControlPlan;
        private DBPrincipal _principal;
        private DelegateCommand<Window> _cancel, _confirm;
        private IEnumerable<ISelectableRequirement> _requirementList;
        private IEnumerable<Person> _techList;
        private IEnumerable<Specification> _specificationList;
        private Int32 _number;
        private Material _material;
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
            _specificationList = SpecificationService.GetSpecifications();

            _confirm = new DelegateCommand<Window>(
                parent => {
                    _reportInstance = new Report();
                    _reportInstance.AuthorID = _author.ID;
                    _reportInstance.BatchID = CommonProcedures.GetBatch(_batchNumber).ID;
                    _reportInstance.Category = "TR";
                    if (_selectedControlPlan != null)
                        _reportInstance.Description = _selectedControlPlan.Name;
                    else
                        _reportInstance.Description = _selectedSpecification.Description;
                    _reportInstance.IsComplete = false;
                    _reportInstance.Number = _number;
                    _reportInstance.SpecificationVersionID = _selectedVersion.ID;
                    _reportInstance.StartDate = DateTime.Now.ToShortDateString();
                    _reportInstance.SpecificationIssueID = _selectedVersion.Specification.Standard.CurrentIssueID;

                    foreach (Test tst in CommonProcedures.GenerateTestList(_requirementList))
                        _reportInstance.Tests.Add(tst);

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

                SelectedBatch = MaterialService.GetBatch(_batchNumber);
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
        
        public string ExternalConstruction
        {
            get
            {
                if (_material == null || _material.Construction.ExternalConstruction == null)
                    return null;

                return _material.Construction.ExternalConstruction.Name;
            }
        }

        public bool IsValidInput
        {
            get { return true; }
        }
        
        public Material Material
        {
            get
            {
                return _material;
            }
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

        public Batch SelectedBatch
        {
            get { return _selectedBatch; }
            set
            {
                _selectedBatch = value;
                _material = _selectedBatch.GetMaterial();

                if (_material != null && _material.Construction.ExternalConstruction != null)
                {
                    SpecificationVersion tempVersion = SpecificationService.GetSpecificationVersion((int)_material.Construction.ExternalConstruction.DefaultSpecVersionID);
                    SelectedSpecification = SpecificationList.FirstOrDefault(spec => spec.ID == tempVersion.SpecificationID);
                    SelectedVersion = VersionList.First(vers => vers.ID == tempVersion.ID);
                }

                RaisePropertyChanged("ExternalConstruction");
                RaisePropertyChanged("Material");
            }
        }

        public ControlPlan SelectedControlPlan
        {
            get { return _selectedControlPlan; }
            set
            {
                _selectedControlPlan = value;
                _selectedControlPlan.Load();

                RaisePropertyChanged("SelectedControlPlan");
                if (value != null)
                {
                    CommonProcedures.ApplyControlPlan(_requirementList, _selectedControlPlan);
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

                RaisePropertyChanged("SelectedSpecification");
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
                _selectedVersion.Load();

                if (_selectedVersion != null)
                {
                    _requirementList = _selectedVersion.GenerateRequirementList()
                                                        .Select(req => new ReportItemWrapper(req))
                                                        .ToList();
                    CommonProcedures.ApplyControlPlan(_requirementList, _selectedControlPlan);
                }

                else
                    _requirementList = new List<ISelectableRequirement>();

                RaisePropertyChanged("RequirementList");
                RaisePropertyChanged("SelectedVersion");
            }
        }
        
        public IEnumerable<Specification> SpecificationList
        {
            get { return _specificationList; }
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
                return _techList;
            }
        }
    }
}