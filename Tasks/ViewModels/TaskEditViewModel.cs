using DBManager;
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
        private DBEntities _entities;
        private DBPrincipal _principal;
        private DelegateCommand _convertToReport;
        private EventAggregator _eventAggregator;
        private DBManager.Task _instance;

        public TaskEditViewModel(DBEntities entities,
                                DBPrincipal principal,
                                EventAggregator aggregator) : base()
        {
            _entities = entities;
            _eventAggregator = aggregator;
            _principal = principal;

            _convertToReport = new DelegateCommand(
                () =>
                {
                    _eventAggregator.GetEvent<TaskToReportConversionRequested>().Publish(_instance);
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

        public string Specification
        {
            get
            {
                if (_instance == null)
                    return null;

                else
                    return _instance.SpecificationVersion.Specification.Standard.Name + " "
                        + _instance.SpecificationVersion.Specification.Standard.CurrentIssue.Issue;
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

        public DBManager.Task TaskInstance
        {
            get { return _instance; }
            set
            {
                _instance = _entities.Tasks.First(tsk => tsk.ID == value.ID);
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
