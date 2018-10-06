using Controls.Views;
using Infrastructure;
using LabDbContext;
using LabDbContext.EntityExtensions;
using LabDbContext.Services;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Tasks.ViewModels
{
    public class TaskEditViewModel : BindableBase
    {
        #region Fields

        private bool _editMode;
        private IEventAggregator _eventAggregator;
        private LabDbContext.Task _instance;
        private IReportService _reportService;

        #endregion Fields

        #region Constructors

        public TaskEditViewModel(IEventAggregator aggregator,
                                IReportService reportService) : base()
        {
            _editMode = false;
            _eventAggregator = aggregator;
            _reportService = reportService;

            ConvertToReportCommand = new DelegateCommand(
                () => _reportService.CreateReport(_instance),
                () => CanCreateReport);

            SaveCommand = new DelegateCommand(
                () =>
                {
                    _instance.Update();
                    EditMode = false;
                },
                () => _editMode);

            StartEditCommand = new DelegateCommand(
                () => EditMode = true,
                () => CanEdit && !_editMode);
        }

        #endregion Constructors

        #region Properties

        public string BatchNumber
        {
            get => _instance?.Batch?.Number;

            set => _instance.Batch.Number = value;
        }

        public bool CanCreateReport => Thread.CurrentPrincipal.IsInRole(UserRoleNames.ReportEdit);

        public bool CanEdit
        {
            get
            {
                if (_instance == null)
                    return false;
                else if ((_instance.IsComplete == true) || _instance.IsAssigned)
                    return false;
                else
                    return ((Thread.CurrentPrincipal as DBPrincipal).CurrentPerson.ID == _instance.Requester.ID)
                        || Thread.CurrentPrincipal.IsInRole(UserRoleNames.TaskAdmin);
            }
        }

        public DelegateCommand ConvertToReportCommand { get; }

        public bool EditMode
        {
            get => _editMode;
            set
            {
                _editMode = value;
                RaisePropertyChanged("EditMode");

                SaveCommand.RaiseCanExecuteChanged();
                StartEditCommand.RaiseCanExecuteChanged();
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

        public DelegateCommand SaveCommand { get; }

        public string SpecificationVersionString => _instance?.SpecificationName + " " + _instance?.SpecificationVersionName;

        public DateTime? StartDate => _instance?.StartDate;

        public DelegateCommand StartEditCommand { get; }

        public string TaskEditProjectDetailsRegionName => RegionNames.TaskEditProjectDetailsRegion;

        public LabDbContext.Task TaskInstance
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

        #endregion Properties
    }
}