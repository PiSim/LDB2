using DBManager;
using DBManager.Services;
using Infrastructure;
using Infrastructure.Wrappers;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Tasks.ViewModels
{
    public class TaskCreationDialogViewModel : BindableBase
    {
        private string _batchNumber, _notes;
        private ControlPlan _selectedControlPlan;
        private DBEntities _entities;
        private DelegateCommand _cancel, _confirm;
        private IMaterialServiceProvider _materialServiceProvider;
        private IReportServiceProvider _reportServiceProvider;
        private List<ControlPlan> _controlPlanList;
        private ObservableCollection<ReportItemWrapper> _requirementList;
        private ObservableCollection<SpecificationVersion> _versionList;
        private Person _requester;
        private Specification _selectedSpecification;
        private SpecificationVersion _selectedVersion;
        private Views.TaskCreationDialog _parentView;

        public TaskCreationDialogViewModel(DBEntities entities,
                                    IMaterialServiceProvider serviceProvider,
                                    IReportServiceProvider reportServiceProvider,
                                    Views.TaskCreationDialog parentView) : base()
        {
            _controlPlanList = new List<ControlPlan>();
            _entities = entities;
            _materialServiceProvider = serviceProvider;
            _reportServiceProvider = reportServiceProvider;
            _parentView = parentView;
            _requirementList = new ObservableCollection<ReportItemWrapper>();
            _versionList = new ObservableCollection<SpecificationVersion>();

            _cancel = new DelegateCommand(
                () => 
                {
                    _parentView.DialogResult = false;
                } );
            
            _confirm = new DelegateCommand(
                () => 
                {
                    Task output = new Task();
                    Batch tempBatch = _materialServiceProvider.GetBatch(_batchNumber);

                    output.Batch = _entities.Batches.First(btc => btc.ID == tempBatch.ID);
                    output.IsComplete = false;
                    output.Notes = _notes;
                    output.PipelineOrder = 0;
                    output.PriorityModifier = 0;
                    output.Progress = 0;
                    output.PriorityModifier = 0;
                    output.Requester = _requester;
                    output.SpecificationVersion = _selectedVersion;
                    output.StartDate = DateTime.Now.Date;

                    foreach (ReportItemWrapper req in _requirementList)
                    {
                        if (!req.IsSelected)
                            continue;

                        TaskItem temp = new TaskItem();
                        temp.Requirement = req.RequirementInstance;
                        output.TaskItems.Add(temp);
                    }
                    
                    _entities.Tasks.Add(output);
                    _entities.SaveChanges();
                    _parentView.TaskInstance = output;
                    _parentView.DialogResult = true;
                },
                () => IsValidInput
            );
            
        }


        public string BatchNumber
        {
            get { return _batchNumber; }
            set { _batchNumber = value; }
        }

        public DelegateCommand CancelCommand
        {
            get { return _cancel; }
        }

        public DelegateCommand ConfirmCommand
        {
            get { return _confirm; }
        }

        public List<ControlPlan> ControlPlanList
        {
            get
            {
                return _controlPlanList;
            }
        }

        public bool IsValidInput
        {
            get { return true; }
        }

        public List<Person> LeaderList
        {
            get
            {
                PersonRole techRole = _entities.PersonRoles.First(prr => prr.Name == PersonRoleNames.ProjectLeader);
                return new List<Person>(techRole.RoleMappings.Where(prm => prm.IsSelected)
                                                            .Select(prm => prm.Person));
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
                    _reportServiceProvider.ApplyControlPlan(_requirementList, _selectedControlPlan);
                }
            }
        }


        public Specification SelectedSpecification
        {
            get { return _selectedSpecification; }
            set
            {
                _selectedSpecification = value;

                if (_selectedSpecification != null)
                {
                    _versionList = new ObservableCollection<SpecificationVersion>(
                        _entities.SpecificationVersions.Where(sv => sv.SpecificationID == _selectedSpecification.ID));
                    RaisePropertyChanged("VersionList");
                    
                    _controlPlanList = new List<ControlPlan>(_selectedSpecification.ControlPlans);
                    RaisePropertyChanged("ControlPlanList");
                }

                else
                {
                    _controlPlanList.Clear();
                    _versionList.Clear();
                }

                SelectedControlPlan = _controlPlanList.FirstOrDefault(cp => cp.IsDefault);
                SelectedVersion = _versionList.FirstOrDefault(sv => sv.IsMain);
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

        public List<Specification> SpecificationList
        {
            get { return new List<Specification>(_entities.Specifications); }
        }

        public ObservableCollection<SpecificationVersion> VersionList
        {
            get { return _versionList; }
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
    }

}