using DBManager;
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
        private string _batchNumber;
        private DBEntities _entities;
        private DelegateCommand _cancel, _confirm;
        private ObservableCollection<ReportItemWrapper> _requirementList;
        private ObservableCollection<SpecificationVersion> _versionList;
        private Person _requester;
        private Specification _selectedSpecification;
        private SpecificationVersion _selectedVersion;
        private Views.TaskCreationDialog _parentView;

        public TaskCreationDialogViewModel(DBEntities entities,
                                    Views.TaskCreationDialog parentView) : base()
        {
            _entities = entities;
            _parentView = parentView;
            
            _cancel = new DelegateCommand(
                () => 
                {
                    _parentView.DialogResult = false;
                } );
            
            _confirm = new DelegateCommand(
                () => 
                {
                    Task output = new Task();
                    output.Requester = _requester;
                    output.SpecificationVersion = _selectedVersion;
                    output.Batch = _entities.GetBatchByNumber(_batchNumber);
                    
                    foreach (ReportItemWrapper req in _requirementList)
                    {
                        TaskItem temp = new TaskItem();
                        temp.Requirement = req.Instance;
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

        public bool IsValidInput
        {
            get { return true; }
        }

        public List<Person> LeaderList
        {
            get { return new List<Person>(_entities.People.Where(pp => pp.Role == "CP")); }
        }

        public Person Requester
        {
            get { return _requester; }
            set { _requester = value; }
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

                    OnPropertyChanged("VersionList");
                }


                else
                    _versionList.Clear();

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
                    List<Requirement> tempReq = _entities.GenerateRequirementList(_selectedVersion);
                    foreach (Requirement rq in tempReq)
                        RequirementList.Add(new ReportItemWrapper(rq));
                }
                OnPropertyChanged("SelectedVersion");
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
        }
    }

}