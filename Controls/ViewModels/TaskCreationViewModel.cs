using DBManager;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Controls.ViewModels
{
    internal class TaskCreationViewModel : BindableBase
    {
        private string _batchNumber;
        private DBEntities _entities;
        private DelegateCommand _cancel, _confirm;
        private ObservableCollection<RequirementWrapper> _requirementList;
        private ObservableCollection<SpecificationVersion> _versionList;
        private Person _requester;
        private Specification _selectedSpecification;
        private SpecificationVersion _selectedVersion;
        private Views.TaskCreationDialog _parentView;

        internal TaskCreationViewModel(DBEntities entities,
                                    Views.TaskCreationDialog parentView) : base()
        {
            _entities = entities;
            _parentView = parentView;
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

                SelectedVersion = _versionList.FirstOrDefault(sv => sv.IsMain == 1);
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
                        RequirementList.Add(new RequirementWrapper(rq));
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

        public ObservableCollection<RequirementWrapper> RequirementList
        {
            get { return _requirementList; }
        }
    }

}