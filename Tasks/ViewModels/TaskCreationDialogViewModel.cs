using DBManager;
using DBManager.EntityExtensions;
using DBManager.Services;
using Infrastructure;
using Infrastructure.Wrappers;
using Prism.Commands;
using Prism.Mvvm;
using Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;

namespace Tasks.ViewModels
{
    public class TaskCreationDialogViewModel : BindableBase, IRequirementSelector
    {
        private Batch _selectedBatch;
        private ControlPlan _selectedControlPlan;
        private DBPrincipal _principal;
        private DelegateCommand<Window> _cancel, _confirm;
        private IEnumerable<Person> _leaderList;
        private IEnumerable<Specification> _specificationList;
        private IEnumerable<ReportItemWrapper> _requirementList;
        private IList<ControlPlan> _controlPlanList;
        private IList<SpecificationVersion> _versionList;
        private Material _material;
        private Person _requester;
        private Specification _selectedSpecification;
        private SpecificationVersion _selectedVersion;
        private string _batchNumber, _notes;
        private Task _taskInstance;

        public TaskCreationDialogViewModel(DBPrincipal principal) : base()
        {
            _principal = principal;

            _leaderList = PeopleService.GetPeople(PersonRoleNames.ProjectLeader);
            _specificationList = SpecificationService.GetSpecifications();
            _requirementList = new List<ReportItemWrapper>();

            _requester = _leaderList.FirstOrDefault(prs => prs.ID == _principal.CurrentPerson.ID);

            _cancel = new DelegateCommand<Window>(
                parent => 
                {
                    parent.DialogResult = false;
                } );
            
            _confirm = new DelegateCommand<Window>(
                parent =>
                {
                    Batch tempBatch = CommonProcedures.GetBatch(_batchNumber);
                    _taskInstance = new Task
                    {
                        BatchID = tempBatch.ID,
                        IsComplete = false,
                        Notes = _notes,
                        PipelineOrder = 0,
                        PriorityModifier = 0,
                        Progress = 0,
                        RequesterID = _requester.ID,
                        SpecificationVersionID = _selectedVersion.ID,
                        StartDate = DateTime.Now.Date,
                        WorkHours = TotalDuration
                    };

                    foreach (TaskItem tItem in CommonProcedures.GenerateTaskItemList(_requirementList
                                                                .Where(req => req.IsSelected)
                                                                .Select(req => req.RequirementInstance)))
                        _taskInstance.TaskItems.Add(tItem);

                    _taskInstance.Create();
                    
                    parent.DialogResult = true;
                },
                parent => IsValidInput
            );
            
        }

        public void OnRequirementSelectionChanged()
        {
            RaisePropertyChanged("TotalDuration");
        }

        public string BatchNumber
        {
            get { return _batchNumber; }
            set
            {
                _batchNumber = value;
                SelectedBatch = MaterialService.GetBatch(_batchNumber);
            }
        }

        public DelegateCommand<Window> CancelCommand => _cancel;

        public DelegateCommand<Window> ConfirmCommand => _confirm;

        public IList<ControlPlan> ControlPlanList => _controlPlanList;

        public string ExternalConstruction
        {
            get
            {
                if (_material == null || _material.ExternalConstruction == null)
                    return null;

                return _material.ExternalConstruction.Name;
            }
        }


        public bool IsSpecificationSelected
        {
            get
            {
                return _selectedSpecification != null;
            }
        }

        public bool IsValidInput
        {
            get { return true; }
        }

        public IEnumerable<Person> LeaderList
        {
            get
            {
                return _leaderList;
            }
        }

        public Material Material => _material;

        public string Notes
        {
            get { return _notes; }
            set
            {
                _notes = value;
            }
        }

        public Person Requester
        {
            get { return _requester; }
            set
            {
                _requester = value;
                RaisePropertyChanged();
            }
        }

        public IEnumerable<ReportItemWrapper> RequirementList => _requirementList;
        
        public Batch SelectedBatch
        {
            get { return _selectedBatch; }
            set
            {
                _selectedBatch = value;
                _material = _selectedBatch.GetMaterial();

                if (_material != null && _material.ExternalConstruction != null)
                {
                    SpecificationVersion tempVersion = SpecificationService.GetSpecificationVersion((int)_material.ExternalConstruction.DefaultSpecVersionID);
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

                Notes = (_selectedControlPlan != null) ? _selectedControlPlan.Name : "";

                RaisePropertyChanged("SelectedControlPlan");
                if (value != null && _requirementList != null)
                    CommonProcedures.ApplyControlPlan(_requirementList, _selectedControlPlan);
            }
        }

        public Specification SelectedSpecification
        {
            get { return _selectedSpecification; }
            set
            {
                _selectedSpecification = value;
                _controlPlanList = _selectedSpecification.GetControlPlans();
                _versionList = _selectedSpecification.GetVersions();
                
                RaisePropertyChanged("ControlPlanList");
                RaisePropertyChanged("IsSpecificationSelected");
                RaisePropertyChanged("SelectedSpecification");
                RaisePropertyChanged("VersionList");

                SelectedControlPlan = ControlPlanList.First(cp => cp.IsDefault);
                SelectedVersion = VersionList.First(sv => sv.IsMain);             
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
                                                        .Select(req => new ReportItemWrapper(req, this))
                                                        .ToList();
                    CommonProcedures.ApplyControlPlan(_requirementList, _selectedControlPlan);
                }

                else
                    _requirementList = new List<ReportItemWrapper>();

                RaisePropertyChanged("RequirementList");
                RaisePropertyChanged("SelectedVersion");
            }
        }

        public IEnumerable<Specification> SpecificationList => _specificationList;

        public IList<SpecificationVersion> VersionList => _versionList;

        public Task TaskInstance => _taskInstance;

        public double TotalDuration => (_requirementList != null) ? _requirementList.Where(req => req.IsSelected)
                                                                                    .Sum(req => req.Duration) : 0;
        
    }
}