using DBManager;
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
    public class TaskServiceProvider : ITaskServiceProvider
    {
        private DBEntities _entities;
        private EventAggregator _eventAggregator;
        private IUnityContainer _container;

        public TaskServiceProvider(DBEntities entities,
                                    EventAggregator aggregator,
                                    IUnityContainer container)
        {
            _entities = entities;
            _eventAggregator = aggregator;
            _container = container;

            _eventAggregator.GetEvent<ReportCompleted>().Subscribe(
                report => UpdateTaskStatus(report.ParentTask));

            _eventAggregator.GetEvent<TaskCreationRequested>().Subscribe(
                token => OnTaskCreationRequested(token));

            _eventAggregator.GetEvent<TaskToReportConversionRequested>().Subscribe(
                target => StartTaskToReportConversion(target));
        }

        public void UpdateTaskStatus(DBManager.Task target)
        {
            if (target.AllItemsAssigned && !target.Reports.Any(rep => !rep.IsComplete))
            {
                DBManager.Task tempTask = _entities.Tasks.First(tsk => tsk.ID == target.ID);
                tempTask.IsComplete = true;
                _eventAggregator.GetEvent<TaskListUpdateRequested>().Publish();
            }
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
