using DBManager;
using Infrastructure;
using Infrastructure.Events;
using Microsoft.Practices.Unity;
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
    public class TaskMainViewModel : BindableBase
    {
        private bool _showAssigned, _showComplete;
        private DBEntities _entities;
        private DBPrincipal _principal;
        private DelegateCommand _newTask, _removeTask;
        private EventAggregator _eventAggregator;
        private DBManager.Task _selectedTask;
        private UnityContainer _container;

        public TaskMainViewModel(DBEntities entities, 
                                    DBPrincipal principal,
                                    EventAggregator eventAggregator,
                                    UnityContainer container) 
            : base()
        {
            _container = container;
            _entities = entities;
            _eventAggregator = eventAggregator;
            _principal = principal;
            _showAssigned = false;
            _showComplete = false;

            _eventAggregator.GetEvent<TaskListUpdateRequested>().Subscribe(() => RaisePropertyChanged("TaskList"));

            _eventAggregator.GetEvent<TaskCompleted>().Subscribe(
                task =>
                {
                    DBManager.Task tempTask = _entities.Tasks.First<DBManager.Task>(tsk => tsk.ID == task.ID);
                    _entities.Entry<DBManager.Task>(tempTask).Reload();
                    RaisePropertyChanged("TaskList");
                });

            _newTask = new DelegateCommand(
                () =>
                {
                    NewTaskToken token = new NewTaskToken();
                    _eventAggregator.GetEvent<TaskCreationRequested>().
                        Publish(token);
                },
                () => CanCreateTask );

            _removeTask = new DelegateCommand(
                () =>
                {
                    _entities.Tasks.Remove(SelectedTask);
                    _entities.SaveChanges();
                    SelectedTask = null;
                    RaisePropertyChanged("TaskList");
                },
                () => CanDeleteTask );
        }

        public bool CanCreateTask
        {
            get { return _principal.IsInRole(UserRoleNames.TaskEdit) || _principal.IsInRole(UserRoleNames.TaskAdmin); }
        }

        public bool CanDeleteTask
        {
            get 
            {
                if (_selectedTask == null)
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
        
        public List<DBManager.Task> TaskList
        {
            get
            {
                if (_showComplete)
                    return new List<DBManager.Task>(_entities.Tasks);

                if (!_showComplete && _showAssigned)
                    return new List<DBManager.Task>(_entities.Tasks.Where(tsk => !tsk.IsComplete));

                else
                    return new List<DBManager.Task>(_entities.Tasks.Where(tsk => !tsk.AllItemsAssigned));
            }
        }
    }
}
