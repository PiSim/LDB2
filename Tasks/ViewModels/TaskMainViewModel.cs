using DBManager;
using DBManager.EntityExtensions;
using DBManager.Services;
using Infrastructure;
using Infrastructure.Events;
using Microsoft.Practices.Unity;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Reporting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace Tasks.ViewModels
{
    public class TaskMainViewModel : BindableBase
    {
        private bool _showAssigned, _showComplete;
        private DBPrincipal _principal;
        private DelegateCommand _newTask, 
                                _removeTask;
        private DelegateCommand<DataGrid> _printTaskList;
        private EventAggregator _eventAggregator;
        private DBManager.Task _selectedTask;
        private IDataService _dataService;
        private IReportingService _reportingService;
        private ITaskService _taskService;

        public TaskMainViewModel(DBPrincipal principal,
                                EventAggregator eventAggregator,
                                IDataService dataService,
                                IReportingService reportingService,
                                ITaskService taskService) 
            : base()
        {
            _dataService = dataService;
            _eventAggregator = eventAggregator;
            _principal = principal;
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

            _newTask = new DelegateCommand(
                () =>
                {
                    _taskService.CreateNewTask();
                },
                () => CanCreateTask );

            _printTaskList = new DelegateCommand<DataGrid>(
                grid =>
                {
                    _reportingService.PrintTaskList(grid.ItemsSource as IEnumerable<Task>);
                });

            _removeTask = new DelegateCommand(
                () =>
                {
                    SelectedTask.Delete();
                    SelectedTask = null;
                    RaisePropertyChanged("TaskList");
                },
                () => CanDeleteTask);
        }

        public bool CanCreateTask
        {
            get
            {
                return _principal.IsInRole(UserRoleNames.TaskEdit) 
                    || _principal.IsInRole(UserRoleNames.TaskAdmin);
            }
        }

        public bool CanDeleteTask
        {
            get 
            {
                if (_selectedTask == null)
                    return false;

                else if (_selectedTask.IsAssigned || (_selectedTask.IsComplete == true))
                    return false;

                else if (_selectedTask.Requester.ID == _principal.CurrentPerson.ID && _principal.IsInRole(UserRoleNames.TaskEdit))
                    return true;

                else if (_principal.IsInRole(UserRoleNames.TaskAdmin))
                    return true;

                else
                    return false;
            }
        }

        public string MainTaskListRegionName
        {
            get { return RegionNames.TaskMainListRegion; }
        }

        public string TaskViewRegionName
        {
            get { return RegionNames.TaskViewRegion; }
        }
        
        public DelegateCommand NewTaskCommand
        {
            get { return _newTask; }
        }

        public DelegateCommand RemoveTaskCommand
        {
            get { return _removeTask; }
        }

        public DelegateCommand<DataGrid> PrintTaskListCommand => _printTaskList;


        public DBManager.Task SelectedTask 
        {
            get { return _selectedTask; }
            set 
            {
                _selectedTask = value;
                RaisePropertyChanged("SelectedTask");
                _removeTask.RaiseCanExecuteChanged();

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
        
        public IEnumerable<DBManager.Task> TaskList
        {
            get
            {
                return _dataService.GetTasks(_showComplete,
                                                _showAssigned);
            }
        }
    }
}
