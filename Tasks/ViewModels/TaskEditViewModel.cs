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
        private DelegateCommand _convertToReport,
                                _save,
                                _startEdit;
        private EventAggregator _eventAggregator;
        private IReportService _reportService;
        private DBManager.Task _instance;

        public TaskEditViewModel(DBPrincipal principal,
                                EventAggregator aggregator,
                                IReportService reportService) : base()
        {
            _editMode = false;
            _eventAggregator = aggregator;
            _principal = principal;
            _reportService = reportService;

            _convertToReport = new DelegateCommand(
                () =>_reportService.CreateReport(_instance),
                () => CanCreateReport);

            _save = new DelegateCommand(
                () =>
                {
                    _instance.Update();
                    EditMode = false;
                },
                () => _editMode);

            _startEdit = new DelegateCommand(
                () => EditMode = true,
                () => CanEdit && !_editMode);
        }

        public string BatchNumber
        {
            get => _instance?.Batch?.Number;

            set => _instance.Batch.Number = value;
        }

        public bool CanCreateReport => _principal.IsInRole(UserRoleNames.ReportEdit);

        public bool CanEdit
        {
            get
            {
                if (_instance == null)
                    return false;

                else if ((_instance.IsComplete == true) || _instance.IsAssigned)
                    return false;

                else
                    return (_principal.CurrentPerson.ID == _instance.Requester.ID)
                        || _principal.IsInRole(UserRoleNames.TaskAdmin);
            }
        }

        public DelegateCommand ConvertToReportCommand => _convertToReport;

        public bool EditMode
        {
            get => _editMode;
            set
            {
                _editMode = value;
                RaisePropertyChanged("EditMode");

                _save.RaiseCanExecuteChanged();
                _startEdit.RaiseCanExecuteChanged();
            }
        }

        public string MaterialConstruction => _instance?.Batch?.Material?.ExternalConstruction?.Name;

        public string MaterialString => _instance?.Batch.Material?.MaterialType.Code
                                        + _instance?.Batch.Material?.MaterialLine.Code
                                        + _instance?.Batch.Material?.Aspect.Code
                                        + _instance?.Batch.Material?.Recipe.Code;

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

        public Project Project => _instance?.Batch?.Material?.Project;

        public string RequesterName => _instance?.Requester?.Name;

        public IEnumerable<TaskItem> RequiredTestList => (_instance != null) ? _instance.GetTaskItems() : new List<TaskItem>();

        public DelegateCommand SaveCommand => _save;

        public string SpecificationVersionString => _instance?.SpecificationName + " " + _instance?.SpecificationVersionName;

        public DateTime? StartDate => _instance?.StartDate;

        public DelegateCommand StartEditCommand => _startEdit;

        public DBManager.Task TaskInstance
        {
            get { return _instance; }
            set
            {
                EditMode = false;

                _instance = value;
                if (_instance != null)
                    _instance.Load();

                RaisePropertyChanged("BatchNumber");
                RaisePropertyChanged("MaterialConstruction");
                RaisePropertyChanged("MaterialString");
                RaisePropertyChanged("Notes");
                RaisePropertyChanged("Project");
                RaisePropertyChanged("RequesterName");
                RaisePropertyChanged("RequiredTestList");
                RaisePropertyChanged("SpecificationVersionString");
                RaisePropertyChanged("StartDate");
            }
        }

        public string TaskEditProjectDetailsRegionName
        {
            get { return RegionNames.TaskEditProjectDetailsRegion; }
        }
    }
}
