using DBManager;
using DBManager.Services;
using Infrastructure;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Projects.ViewModels
{
    internal class ProjectCreationDialogViewModel : BindableBase
    {
        private DelegateCommand<Window> _cancel, _confirm;
        private Organization _selectedOem;
        private Person _selectedLeader;
        private Project _projectInstance;
        private string _description, _name;
        
        internal ProjectCreationDialogViewModel(DBEntities entities) : base()
        {

            _cancel = new DelegateCommand<Window>(
                parent =>
                {
                    parent.DialogResult = false;
                });

            _confirm = new DelegateCommand<Window>(
                parent =>
                {
                    _projectInstance = new Project();
                    _projectInstance.Description = _description;
                    _projectInstance.Leader = _selectedLeader;
                    _projectInstance.Name = _name;
                    _projectInstance.Oem = _selectedOem;

                    _projectInstance.Create();
                    
                    parent.DialogResult = true;
                },
                parent => IsValidInput);
        }

        public DelegateCommand<Window> CancelCommand
        {
            get { return _cancel; }
        }

        public DelegateCommand<Window> ConfirmCommand
        {
            get { return _confirm; }
        }

        public bool IsValidInput
        {
            get
            {
                return _description != null &&
                      _name != null &&
                      _selectedLeader != null &&
                      _selectedOem != null;
            }
        }

        public IEnumerable<Person> LeaderList
        {
            get
            {
                return PeopleService.GetProjectLeaders();
            }
        }

        public IEnumerable<Organization> OemList
        {
            get { return OrganizationService.GetOrganizations(OrganizationRoleNames.OEM); }
        }

        public string ProjectDescription
        {
            get { return _description; }
            set
            {
                _description = value;
                _confirm.RaiseCanExecuteChanged();
            }
        }

        public Project ProjectInstance
        {
            get { return _projectInstance; }
        }

        public string ProjectName
        {
            get { return _name; }
            set
            {
                _name = value;
                _confirm.RaiseCanExecuteChanged();
            }
        }

        public Person SelectedLeader
        {
            get { return _selectedLeader; }
            set
            {
                _selectedLeader = value;
                _confirm.RaiseCanExecuteChanged();
            }
        }

        public Organization SelectedOem
        {
            get { return _selectedOem; }
            set
            {
                _selectedOem = value;
                _confirm.RaiseCanExecuteChanged();
            }
        }
    }
}
