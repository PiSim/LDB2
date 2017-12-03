using DBManager;
using DBManager.Services;
using Infrastructure;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Projects.ViewModels
{
    public class ProjectCreationDialogViewModel : BindableBase, INotifyDataErrorInfo
    {
        private DelegateCommand<Window> _cancel, _confirm;
        private readonly Dictionary<string, ICollection<string>> _validationErrors = new Dictionary<string, ICollection<string>>();
        private IDataService _dataService;
        private Organization _selectedOem;
        private Person _selectedLeader;
        private Project _projectInstance;
        private string _description, _name;
        
        public ProjectCreationDialogViewModel(IDataService dataService) : base()
        {
            _dataService = dataService;
            _description = "";

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
                    _projectInstance.ProjectLeaderID = _selectedLeader.ID;
                    _projectInstance.Name = _name;
                    _projectInstance.OemID = _selectedOem.ID;

                    _projectInstance.Create();
                    
                    parent.DialogResult = true;
                },
                parent => !HasErrors);

            ProjectName = "";
            SelectedLeader = null;
            SelectedOem = null;
        }

        #region INotifyDataErrorInfo interface elements

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName)
                || !_validationErrors.ContainsKey(propertyName))
                return null;

            return _validationErrors[propertyName];
        }

        public bool HasErrors
        {
            get { return _validationErrors.Count > 0; }
        }

        private void RaiseErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            _confirm.RaiseCanExecuteChanged();
        }

        #endregion

        public DelegateCommand<Window> CancelCommand
        {
            get { return _cancel; }
        }

        public DelegateCommand<Window> ConfirmCommand
        {
            get { return _confirm; }
        }

        public IEnumerable<Person> LeaderList => _dataService.GetPeople(PersonRoleNames.ProjectLeader);

        public IEnumerable<Organization> OemList => _dataService.GetOrganizations(OrganizationRoleNames.OEM);

        public string ProjectDescription
        {
            get { return _description; }
            set
            {
                _description = value;
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
                if (!string.IsNullOrEmpty(_name) && DBManager.Services.ProjectService.GetProject(_name) == null)
                {
                    if (_validationErrors.ContainsKey("ProjectName"))
                    {
                        _validationErrors.Remove("ProjectName");
                        RaiseErrorsChanged("ProjectName");
                    }
                }
                else
                {
                    _validationErrors["ProjectName"] = new List<string>() { "Nome non valido" };
                    RaiseErrorsChanged("ProjectName");
                }
            }
        }

        public Person SelectedLeader
        {
            get { return _selectedLeader; }
            set
            {
                _selectedLeader = value;
                if (_selectedLeader != null)
                {
                    if (_validationErrors.ContainsKey("SelectedLeader"))
                    {
                        _validationErrors.Remove("SelectedLeader");
                        RaiseErrorsChanged("SelectedLeader");
                    }
                }
                else
                {
                    _validationErrors["SelectedLeader"] = new List<string>() { "Selezionare un capo progetto" };
                    RaiseErrorsChanged("SelectedLeader");
                }
            }
        }

        public Organization SelectedOem
        {
            get { return _selectedOem; }
            set
            {
                _selectedOem = value;
                if (_selectedOem != null)
                {
                    if (_validationErrors.ContainsKey("SelectedOem"))
                    {
                        _validationErrors.Remove("SelectedOem");
                        RaiseErrorsChanged("SelectedOem");
                    }
                }
                else
                {
                    _validationErrors["SelectedOem"] = new List<string>() { "Selezionare un OEM" };
                    RaiseErrorsChanged("SelectedOem");
                }
            }
        }
    }
}
