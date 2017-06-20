using DBManager;
using DBManager.EntityExtensions;
using Infrastructure;
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

            _eventAggregator.GetEvent<TaskToReportConversionRequested>().Subscribe(
                target => StartTaskToReportConversion(target));

            _eventAggregator.GetEvent<TaskStatusCheckRequested>().Subscribe(
                taskEntry => UpdateTaskStatus(taskEntry));
        }

        public void UpdateTaskStatus(DBManager.Task target)
        {
            target.Load();

            target.AllItemsAssigned = target.TaskItems.All(tski => tski.Test != null);

            if (target.AllItemsAssigned)
                target.IsComplete = target.TaskItems.All(tski => tski.Test.IsComplete);

            else
                target.IsComplete = false;

            target.Update();
        }

        private void OnTaskCreationRequested(NewTaskToken token)
        {
            Views.TaskCreationDialog creationDialog = 
                _container.Resolve<Views.TaskCreationDialog>();

            if (creationDialog.ShowDialog() == true)
            {
                _eventAggregator.GetEvent<TaskListUpdateRequested>().Publish();
            }
        }

        public Report StartTaskToReportConversion(DBManager.Task target)
        {
            Views.ConversionReviewDialog conversionDialog = _container.Resolve<Views.ConversionReviewDialog>();
            conversionDialog.TaskInstance = target;

            if (conversionDialog.ShowDialog() == true)
            {
                _eventAggregator.GetEvent<ReportListUpdateRequested>().Publish();

                NavigationToken token = new NavigationToken(ReportViewNames.ReportEditView,
                                                            conversionDialog.ReportInstance);
                _eventAggregator.GetEvent<NavigationRequested>().Publish(token);
                
                return conversionDialog.ReportInstance;
            }

            return null;
        }
    }
}
