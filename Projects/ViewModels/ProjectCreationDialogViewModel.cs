using DataAccess;
using Infrastructure.Commands;
using Infrastructure.Queries;
using LabDbContext;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace Projects.ViewModels
{
    public class ProjectCreationDialogViewModel : BindableBase, INotifyDataErrorInfo
    {
        #region Fields

        private readonly Dictionary<string, ICollection<string>> _validationErrors = new Dictionary<string, ICollection<string>>();
        private IDataService<LabDbEntities> _labDbData;
        private string _name;
        private Person _selectedLeader;
        private Organization _selectedOem;

        #endregion Fields

        #region Constructors

        public ProjectCreationDialogViewModel(IDataService<LabDbEntities> labDbData) : base()
        {
            _labDbData = labDbData;
            ProjectDescription = "";

            CancelCommand = new DelegateCommand<Window>(
                parent =>
                {
                    parent.DialogResult = false;
                });

            ConfirmCommand = new DelegateCommand<Window>(
                parent =>
                {
                    ProjectInstance = new Project();
                    ProjectInstance.Description = ProjectDescription;
                    ProjectInstance.ProjectLeaderID = _selectedLeader.ID;
                    ProjectInstance.Name = _name;
                    ProjectInstance.OemID = _selectedOem.ID;

                    _labDbData.Execute(new InsertEntityCommand(ProjectInstance));

                    parent.DialogResult = true;
                },
                parent => !HasErrors);

            ProjectName = "";
            SelectedLeader = null;
            SelectedOem = null;
        }

        #endregion Constructors

        #region INotifyDataErrorInfo interface elements

        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;

        public bool HasErrors => _validationErrors.Count > 0;

        public IEnumerable GetErrors(string propertyName)
        {
            if (string.IsNullOrEmpty(propertyName)
                || !_validationErrors.ContainsKey(propertyName))
                return null;

            return _validationErrors[propertyName];
        }

        private void RaiseErrorsChanged(string propertyName)
        {
            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propertyName));
            ConfirmCommand.RaiseCanExecuteChanged();
        }

        #endregion INotifyDataErrorInfo interface elements

        #region Properties

        public DelegateCommand<Window> CancelCommand { get; }

        public DelegateCommand<Window> ConfirmCommand { get; }

        public IEnumerable<Person> LeaderList => _labDbData.RunQuery(new PeopleQuery() { Role = PeopleQuery.PersonRoles.ProjectLeader })
                                                            .ToList();

        public IEnumerable<Organization> OemList => _labDbData.RunQuery(new OrganizationsQuery() { Role = OrganizationsQuery.OrganizationRoles.OEM })
                                                                        .ToList();

        public string ProjectDescription { get; set; }

        public Project ProjectInstance { get; private set; }

        public string ProjectName
        {
            get { return _name; }
            set
            {
                _name = value;
                if (!string.IsNullOrEmpty(_name) && !_labDbData.RunQuery(new ProjectsQuery() { IncludeCollections = false }).Any(prj => prj.Name == _name))
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

        #endregion Properties
    }
}