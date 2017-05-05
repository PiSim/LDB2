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
        private DBEntities _entities;
        private DelegateCommand _newTask;
        private EventAggregator _eventAggregator;
        private DBManager.Task _selectedTask;
        private UnityContainer _container;

        public TaskMainViewModel(DBEntities entities, 
                                    EventAggregator eventAggregator,
                                    UnityContainer container) 
            : base()
        {
            _container = container;
            _entities = entities;
            _eventAggregator = eventAggregator;

            _eventAggregator.GetEvent<TaskListUpdateRequested>().Subscribe(() => RaisePropertyChanged("TaskList"));

            _newTask = new DelegateCommand(
                () =>
                {
                    NewTaskToken token = new NewTaskToken();
                    _eventAggregator.GetEvent<TaskCreationRequested>().
                        Publish(token);
                } );
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
        
        public DBManager.Task SelectedTask 
        {
            get { return _selectedTask; }
            set 
            {
                _selectedTask = value;
                RaisePropertyChanged("SelectedTask");

                NavigationToken token = new NavigationToken(TaskViewNames.TaskEditView,
                                                            _selectedTask,
                                                            RegionNames.TaskViewRegion);

                _eventAggregator.GetEvent<NavigationRequested>().Publish(token);
            }       
        }
        
        public List<DBManager.Task> TaskList
        {
            get { return new List<DBManager.Task>(_entities.Tasks); }
        }
    }
}
