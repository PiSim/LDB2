using Controls.Views;
using Infrastructure;
using Infrastructure.Events;
using LabDbContext;
using LabDbContext.EntityExtensions;
using LabDbContext.Services;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Reporting;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Controls;

namespace Tasks.ViewModels
{
    public class TaskMainViewModel : BindableBase
    {
        #region Fields
        
        private IEventAggregator _eventAggregator;
        private IReportingService _reportingService;
        private LabDbContext.Task _selectedTask;
        private bool _showAssigned, _showComplete;
        private ITaskService _taskService;

        #endregion Fields

        #region Constructors

        public TaskMainViewModel(IEventAggregator eventAggregator,
                                IReportingService reportingService,
                                ITaskService taskService)
            : base()
        {
            _eventAggregator = eventAggregator;
            _reportingService = reportingService;
            _showAssigned = false;
            _showComplete = false;
            _taskService = taskService;

            _eventAggregator.GetEvent<TaskListUpdateRequested>().Subscribe(() => RaisePropertyChanged("TaskList"));

            _eventAggregator.GetEvent<TaskCompleted>().Subscribe(
                task =>
                {
                    RaisePropertyChanged("TaskList");
                });

            NewTaskCommand = new DelegateCommand(
                () =>
                {
                    _taskService.CreateNewTask();
                },
                () => CanCreateTask);

            PrintTaskListCommand = new DelegateCommand<DataGrid>(
                grid =>
                {
                    _reportingService.PrintTaskList(grid.ItemsSource as IEnumerable<Task>);
                });

            RemoveTaskCommand = new DelegateCommand(
                () =>
                {
                    SelectedTask.Delete();
                    SelectedTask = null;
                    RaisePropertyChanged("TaskList");
                },
                () => CanDeleteTask);
        }

        #endregion Constructors

        #region Properties

        public bool CanCreateTask => Thread.CurrentPrincipal.IsInRole(UserRoleNames.TaskEdit)
                    || Thread.CurrentPrincipal.IsInRole(UserRoleNames.TaskAdmin);

        public bool CanDeleteTask
        {
            get
            {
                if (_selectedTask == null)
                    return false;
                else if (_selectedTask.IsAssigned || (_selectedTask.IsComplete == true))
                    return false;
                else if (_selectedTask.Requester.ID == (Thread.CurrentPrincipal as DBPrincipal).CurrentPerson.ID && Thread.CurrentPrincipal.IsInRole(UserRoleNames.TaskEdit))
                    return true;
                else if (Thread.CurrentPrincipal.IsInRole(UserRoleNames.TaskAdmin))
                    return true;
                else
                    return false;
            }
        }

        public string MainTaskListRegionName => RegionNames.TaskMainListRegion;

        public DelegateCommand NewTaskCommand { get; }
        public DelegateCommand<DataGrid> PrintTaskListCommand { get; }
        public DelegateCommand RemoveTaskCommand { get; }

        public LabDbContext.Task SelectedTask
        {
            get { return _selectedTask; }
            set
            {
                _selectedTask = value;
                RaisePropertyChanged("SelectedTask");
                RemoveTaskCommand.RaiseCanExecuteChanged();

                NavigationToken token = new NavigationToken(TaskViewNames.TaskEditView,
                                                            _selectedTask,
                                                            RegionNames.TaskViewRegion);

                _eventAggregator.GetEvent<NavigationRequested>().Publish(token);
            }
        }

        public bool ShowAssigned
        {
            get { return _showAssigned; }
            set
            {
                _showAssigned = value;

                if (!_showAssigned && _showComplete)
                    ShowComplete = false;

                RaisePropertyChanged();
                RaisePropertyChanged("TaskList");
            }
        }

        public bool ShowComplete
        {
            get { return _showComplete; }
            set
            {
                _showComplete = value;

                if (_showComplete && !_showAssigned)
                    ShowAssigned = true;

                RaisePropertyChanged();
                RaisePropertyChanged("TaskList");
            }
        }

        public IEnumerable<LabDbContext.Task> TaskList => _dataService.GetTasks(_showComplete,
                                                _showAssigned);

        public string TaskViewRegionName => RegionNames.TaskViewRegion;

        #endregion Properties
    }
}