using Infrastructure;
using Infrastructure.Events;
using LabDbContext;
using Prism.Events;
using System.Data.Entity.Infrastructure;

namespace Tasks
{
    public class TaskService : ITaskService
    {
        #region Fields

        private IDbContextFactory<LabDbEntities> _dbContextFactory;
        private IEventAggregator _eventAggregator;

        #endregion Fields

        #region Constructors

        public TaskService(IDbContextFactory<LabDbEntities> dbContextFactory,
                            IEventAggregator aggregator)
        {
            _eventAggregator = aggregator;
            _dbContextFactory = dbContextFactory;
        }

        #endregion Constructors

        #region Methods

        public void CreateNewTask()
        {
            Views.TaskCreationDialog creationDialog = new Views.TaskCreationDialog();

            if (creationDialog.ShowDialog() == true)
                _eventAggregator.GetEvent<TaskListUpdateRequested>().Publish();
        }

        #endregion Methods
    }
}