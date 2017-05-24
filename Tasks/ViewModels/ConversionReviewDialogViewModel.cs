using DBManager;
using DBManager.Services;
using Infrastructure;
using Infrastructure.Wrappers;
using Prism.Commands;
using Prism.Mvvm;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Tasks.ViewModels
{
    public class ConversionReviewDialogViewModel : BindableBase
    {
        private DBPrincipal _principal;
        private DelegateCommand<Window> _cancel, _confirm;
        private IEnumerable<Person> _techList;
        private int _reportNumber;
        private List<TaskItemWrapper> _testList;
        private Person _selectedAuthor;
        private Report _reportInstance;
        private string _notes;
        private DBManager.Task _taskInstance;

        public ConversionReviewDialogViewModel(DBPrincipal principal) : base()
        {
            _principal = principal;
            _techList = PeopleService.GetPeople(PersonRoleNames.MaterialTestingTech);
            _selectedAuthor = _techList.First(pt => pt.ID == _principal.CurrentPerson.ID);

            _testList = new List<TaskItemWrapper>();

            _cancel = new DelegateCommand<Window>(
                parentDialog =>
                {
                    parentDialog.DialogResult = false;
                });

            _confirm = new DelegateCommand<Window>(
                parentDialog =>
                {
                    _reportInstance = new Report();
                    _reportInstance.Author = _selectedAuthor;
                    _reportInstance.Batch = _taskInstance.Batch;
                    _reportInstance.Category = "TR";
                    _reportInstance.Description = _notes;
                    _reportInstance.IsComplete = false;
                    _reportInstance.Number = _reportNumber;
                    _reportInstance.ParentTask = _taskInstance;
                    _reportInstance.SpecificationIssues = _taskInstance.SpecificationVersion.Specification.Standard.CurrentIssue;
                    _reportInstance.SpecificationVersion = _taskInstance.SpecificationVersion;
                    _reportInstance.StartDate = DateTime.Now.Date.ToShortDateString();

                    foreach (Test tst in CommonServices.GenerateTestList(_testList))
                        _reportInstance.Tests.Add(tst);

                    foreach (TaskItemWrapper riw in _testList.Where(tiw => tiw.IsSelected))
                        riw.TaskItemInstance.IsAssignedToReport = true;

                    if (!_taskInstance.TaskItems.Any(tski => !tski.IsAssignedToReport))
                        _taskInstance.AllItemsAssigned = true;

                    _reportInstance.Create();

                    parentDialog.DialogResult = true;
                });
        }

        public string BatchNumber
        {
            get
            {
                if (_taskInstance == null)
                    return null;

                return _taskInstance.Batch.Number;
            }
        }

        public DelegateCommand<Window> CancelCommand
        {
            get { return _cancel; }
        }

        public DelegateCommand<Window> ConfirmCommand
        {
            get { return _confirm; }
        }

        public bool CanChangeAuthor
        {
            get { return _principal.IsInRole(UserRoleNames.ReportAdmin); }
        }

        public string Notes
        {
            get { return _notes; }
            set
            {
                _notes = value;
                RaisePropertyChanged("Notes");
            }
        }

        public int ReportNumber
        {
            get { return _reportNumber; }
            set
            {
                _reportNumber = value;
                RaisePropertyChanged("ReportNumber");
            }
        }

        public Report ReportInstance
        {
            get { return _reportInstance; }
        }

        public Person SelectedAuthor
        {
            get { return _selectedAuthor; }
            set
            {
                _selectedAuthor = value;
                RaisePropertyChanged("SelectedAuthor");
            }
        }

        public string Specification
        {
            get
            {
                if (_taskInstance == null)
                    return null;
                else
                   return _taskInstance.SpecificationVersion.Specification.Standard.Name + " : "
                        + _taskInstance.SpecificationVersion.Specification.Standard.CurrentIssue.Issue;
            }
        }

        public string SpecificationVersion
        {
            get
            {
                if (_taskInstance == null)
                    return null;
                else
                    return _taskInstance.SpecificationVersion.Name;
            }
        }

        public DBManager.Task TaskInstance
        {
            get { return _taskInstance; }
            set
            {
                _taskInstance = value;
                _taskInstance.Load();

                _testList = new List<TaskItemWrapper>();
                foreach (TaskItem tsi in _taskInstance.TaskItems.Where(tsi => !tsi.IsAssignedToReport))
                    _testList.Add(new TaskItemWrapper(tsi));

                _notes = _taskInstance.Notes;

                RaisePropertyChanged("BatchNumber");
                RaisePropertyChanged("Notes");
                RaisePropertyChanged("Specification");
                RaisePropertyChanged("SpecificationVersion");
                RaisePropertyChanged("TaskInstance");
                RaisePropertyChanged("TestList");
            }
        }

        public IEnumerable<Person> TechList
        {
            get
            {
                return _techList;
            }
        }

        public List<TaskItemWrapper> TestList
        {
            get { return _testList; }
        }
    }
}
