using DBManager;
using Infrastructure.Events;
using Microsoft.Practices.Unity;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tasks
{
    public class TaskServiceProvider
    {
        private EventAggregator _eventAggregator;
        private IUnityContainer _container;

        public TaskServiceProvider(EventAggregator aggregator,
                                    IUnityContainer container)
        {
            _eventAggregator = aggregator;
            _container = container;


            _eventAggregator.GetEvent<TaskCreationRequested>().Subscribe(
                token => OnTaskCreationRequested(token));
        }

        private void OnTaskCreationRequested(NewTaskToken token)
        {
            Views.TaskCreationDialog creationDialog = 
                _container.Resolve<Views.TaskCreationDialog>();

            if (creationDialog.ShowDialog() == true)
            {
                _eventAggregator.GetEvent<TaskCreated>().Publish(creationDialog.TaskInstance);
            }
        }
    }
}
