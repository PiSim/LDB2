using DBManager;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projects.ViewModels
{
    public class ModifyProjectDetailsDialogViewModel : BindableBase
    {
        private DBEntities _entities;
        private DelegateCommand _cancel, _confirm;
        private Project _projectInstance;
        private Views.ModifyProjectDetailsDialog _parentDialog;

        public ModifyProjectDetailsDialogViewModel(DBEntities entities,
                                                Views.ModifyProjectDetailsDialog parentDialog) : base()
        {
            _entities = entities;
            _parentDialog = parentDialog;

            _cancel = new DelegateCommand(
                () =>
                {
                    _parentDialog.DialogResult = false;
                });

            _confirm = new DelegateCommand(
                () =>
                {
                    _entities.SaveChanges();
                    _parentDialog.DialogResult = true;
                });
        }

        public DelegateCommand CancelCommand
        {
            get { return _cancel; }
        }

        public DelegateCommand ConfirmCommand
        {
            get { return _confirm; }
        }

        public List<Person> LeaderList
        {
            get
            {
                return new List<Person>(_entities.People.Where(per => per.Role == "CP"));
            }
        }

        public List<Organization> OemList
        {
            get
            {
                return new List<Organization>(_entities.Organizations.Where(org => org.Category == "OEM"));
            }
        }

        public string ProjectDescription
        {
            get
            {
                if (_projectInstance == null)
                    return null;

                return _projectInstance.Description;
            }
            set
            {
                _projectInstance.Description = value;
            }
        }

        public Project ProjectInstance
        {
            get { return _projectInstance; }
            set
            {
                _projectInstance = _entities.Projects.First(prj => prj.ID == value.ID);
                OnPropertyChanged("ProjectDescription");
                OnPropertyChanged("ProjectName");
                OnPropertyChanged("SelectedLeader");
                OnPropertyChanged("SelectedOem");
            }
        }

        public string ProjectName
        {
            get
            {
                if (_projectInstance == null)
                    return null;

                return _projectInstance.Name;
            }
            set
            {
                _projectInstance.Name = value;
            }
        }

        public Person SelectedLeader
        {
            get
            {
                if (_projectInstance == null)
                    return null;

                return _projectInstance.Leader;
            }
            set
            {
                _projectInstance.Leader = value;
            }
        }

        public Organization SelectedOem
        {
            get
            {
                if (_projectInstance == null)
                    return null;

                return _projectInstance.Oem;
            }
            set
            {
                _projectInstance.Oem = value;
            }
        }
    }
}
