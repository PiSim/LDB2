using DBManager;
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
    public class TaskCreationDialogViewModel : BindableBase
    {
        private string _batchNumber, _notes;
        private ControlPlan _selectedControlPlan;
        private DelegateCommand<Window> _cancel, _confirm;
        private IEnumerable<Person> _leaderList;
        private IEnumerable<Specification> _specificationList;
        private ObservableCollection<ReportItemWrapper> _requirementList;
        private Person _requester;
        private Specification _selectedSpecification;
        private SpecificationVersion _selectedVersion;
        private Task _taskInstance;

        public TaskCreationDialogViewModel() : base()
        {
            _leaderList = PeopleService.GetPeople(PersonRoleNames.ProjectLeader);
            _requirementList = new ObservableCollection<ReportItemWrapper>();
            _specificationList = SpecificationService.GetSpecifications();

            _cancel = new DelegateCommand<Window>(
                parent => 
                {
                    parent.DialogResult = false;
                } );
            
            _confirm = new DelegateCommand<Window>(
                parent => 
                {
                    _taskInstance = new Task();
                    Batch tempBatch = MaterialService.GetBatch(_batchNumber);

                    if (tempBatch == null)
                    {
                        tempBatch = new Batch();
                        tempBatch.Number = _batchNumber;
                        tempBatch.Create();
                    }
                    
                    _taskInstance.IsComplete = false;
                    _taskInstance.Notes = _notes;
                    _taskInstance.PipelineOrder = 0;
                    _taskInstance.PriorityModifier = 0;
                    _taskInstance.Progress = 0;
                    _taskInstance.PriorityModifier = 0;
                    _taskInstance.Requester = _requester;
                    _taskInstance.SpecificationVersion = _selectedVersion;
                    _taskInstance.StartDate = DateTime.Now.Date;

                    foreach (ReportItemWrapper req in _requirementList)
                    {
                        if (!req.IsSelected)
                            continue;

                        TaskItem temp = new TaskItem();
                        temp.Requirement = req.RequirementInstance;
                        _taskInstance.TaskItems.Add(temp);
                    }

                    _taskInstance.Create();
                    
                    parent.DialogResult = true;
                },
                parent => IsValidInput
            );
            
        }


        public string BatchNumber
        {
            get { return _batchNumber; }
            set { _batchNumber = value; }
        }

        public DelegateCommand<Window> CancelCommand
        {
            get { return _cancel; }
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

        public IEnumerable<Person> LeaderList
        {
            get
            {
                return _leaderList;
            }
        }

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

        public ControlPlan SelectedControlPlan
        {
            get { return _selectedControlPlan; }
            set
            {
                _selectedControlPlan = value;
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
                RequirementList.Clear();

                if (_selectedVersion != null)
                {
                    List<Requirement> tempReq = ReportService.GenerateRequirementList(_selectedVersion);
                    foreach (Requirement rq in tempReq)
                        RequirementList.Add(new ReportItemWrapper(rq));
                }
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

        public ObservableCollection<ReportItemWrapper> RequirementList
        {
            get { return _requirementList; }
            set
            {
                _requirementList = value;
                RaisePropertyChanged("RequirementList");
            }
        }

        public Task TaskInstance
        {
            get { return _taskInstance; }
        }
    }

}