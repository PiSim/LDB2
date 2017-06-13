using DBManager;
using DBManager.EntityExtensions;
using DBManager.Services;
using Infrastructure;
using Infrastructure.Events;
using Infrastructure.Wrappers;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tasks.ViewModels
{
    public class TaskEditViewModel : BindableBase
    {
        private bool _editMode;
        private DBPrincipal _principal;
        private DelegateCommand _addTest,
                                _convertToReport,
                                _save,
                                _startEdit;
        private EventAggregator _eventAggregator;
        private DBManager.Task _instance;

        public TaskEditViewModel(DBPrincipal principal,
                                EventAggregator aggregator) : base()
        {
            _editMode = false;
            _eventAggregator = aggregator;
            _principal = principal;

            _convertToReport = new DelegateCommand(
                () =>
                {
                    _eventAggregator.GetEvent<TaskToReportConversionRequested>()
                                    .Publish(_instance);
                },
                () => CanCreateReport);

            _startEdit = new DelegateCommand(
                () =>
                {
                    EditMode = true;
                });
        }

        public string BatchNumber
        {
            get
            {
                if (_instance == null)
                    return null;

                else
                    return _instance.Batch.Number;
            }
        }

        public bool CanCreateReport
        {
            get
            {
                return _principal.IsInRole(UserRoleNames.ReportEdit) 
                    || _principal.IsInRole(UserRoleNames.ReportAdmin);
            }
        }

        public bool CanEdit
        {
            get
            {
                if (_instance == null)
                    return false;

                else if (_instance.IsComplete)
                    return false;

                else
                    return (_principal.CurrentPerson.ID == _instance.Requester.ID)
                        || _principal.IsInRole(UserRoleNames.TaskAdmin);
            }
        }

        public DelegateCommand ConvertToReportCommand
        {
            get { return _convertToReport; }
        }

        public bool EditMode
        {
            get { return _editMode; }
            set
            {
                _editMode = value;
                RaisePropertyChanged("EditMode");
            }
        }

        public string Notes
        {
            get
            {
                if (_instance == null)
                    return null;
                else
                    return _instance.Notes;
            }
            set
            {
                _instance.Notes = value;
            }
        }

        public Project Project
        {
            get
            {
                if (_instance == null)
                    return null;
                else
                    return _instance.Batch.Material.Construction.Project;
            }
        }

        public string Requester
        {
            get
            {
                if (_instance == null)
                    return null;

                else
                    return _instance.Requester.Name;
            }
        }

        public List<TaskItem> RequiredTestList
        {
            get
            {
                if (_instance == null)
                    return new List<TaskItem>();
                else
                    return new List<TaskItem>(_instance.TaskItems);
            }
        }

        public DelegateCommand SaveCommand
        {
            get { return _save; }
        }

        public string Specification
        {
            get
            {
                if (_instance == null)
                    return null;

                else
                    return _instance.SpecificationVersion.Specification.Standard.Name + " : "
                        + ((_instance.SpecificationVersion.Specification.Standard.CurrentIssue != null) 
                            ? _instance.SpecificationVersion.Specification.Standard.CurrentIssue.Issue : " ");
            }
        }

        public string SpecificationVersion
        {
            get
            {
                if (_instance == null)
                    return null;

                else
                    return _instance.SpecificationVersion.Name;
            }
        }

        public DateTime StartDate
        {
            get
            {
                if (_instance == null)
                    return DateTime.Now;
                else
                    return _instance.StartDate;
            }
        }

        public DelegateCommand StartEditCommand
        {
            get { return _startEdit; }
        }

        public DBManager.Task TaskInstance
        {
            get { return _instance; }
            set
            {
                _instance = value;
                _instance.Load();

                RaisePropertyChanged("BatchNumber");
                RaisePropertyChanged("CanEdit");
                RaisePropertyChanged("Notes");
                RaisePropertyChanged("Project");
                RaisePropertyChanged("Requester");
                RaisePropertyChanged("RequiredTestList");
                RaisePropertyChanged("Specification");
                RaisePropertyChanged("SpecificationVersion");
                RaisePropertyChanged("StartDate");
            }
        }

        public string TaskEditProjectDetailsRegionName
        {
            get { return RegionNames.TaskEditProjectDetailsRegion; }
        }
    }
}
