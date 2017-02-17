using DBManager;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projects.ViewModels
{
    internal class ProjectCreationViewModel : BindableBase
    {
        private DBEntities _entities;
        private DelegateCommand _cancel, _confirm;
        private Organization _selectedOem;
        private Person _selectedLeader;
        private string _description, _name;
        private Views.ProjectCreationDialog _parent;

        internal ProjectCreationViewModel(DBEntities entities,
                                        Views.ProjectCreationDialog parent) : base()
        {
            _entities = entities;
            _parent = parent;

            _cancel = new DelegateCommand(
                () =>
                {
                    _parent.DialogResult = false;
                });

            _confirm = new DelegateCommand(
                () =>
                {
                    Project temp = new Project();
                    temp.Description = _description;
                    temp.Leader = _selectedLeader;
                    temp.Name = _name;
                    temp.Oem = _selectedOem;

                    _entities.Projects.Add(temp);
                    _entities.SaveChanges();

                    _parent.ProjectInstance = temp;
                    _parent.DialogResult = true;
                },
                () => IsValidInput);
        }

        public DelegateCommand CancelCommand
        {
            get { return _cancel; }
        }

        public DelegateCommand ConfirmCommand
        {
            get { return _confirm; }
        }

        public string Description
        {
            get { return _description; }
            set
            {
                _description = value;
                _confirm.RaiseCanExecuteChanged();
            }
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

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                _confirm.RaiseCanExecuteChanged();
            }
        }

        public List<Person> LeaderList
        {
            get { return new List<Person>(_entities.People.Where(per => per.Role == "CP")); }
        }

        public List<Organization> OemList
        {
            get { return new List<Organization>(_entities.Organizations.Where(org => org.Category == "OEM")); }
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
