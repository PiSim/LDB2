using DBManager;
using DBManager.EntityExtensions;
using Infrastructure;
using Infrastructure.Events;
using Microsoft.Practices.Unity;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Tasks
{
    public class TaskService : ITaskService
    {
        private EventAggregator _eventAggregator;
        private IUnityContainer _container;

        public TaskService(EventAggregator aggregator,
                           IUnityContainer container)
        {
            _eventAggregator = aggregator;
            _container = container;
        }

        public void CreateNewTask()
        {
            Views.TaskCreationDialog creationDialog =
                _container.Resolve<Views.TaskCreationDialog>();

            if (creationDialog.ShowDialog() == true)
                _eventAggregator.GetEvent<TaskListUpdateRequested>().Publish();
        }
    }
}
