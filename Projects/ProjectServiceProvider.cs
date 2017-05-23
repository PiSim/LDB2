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

namespace Projects
{
    public class ProjectServiceProvider : IProjectServiceProvider
    {
        private DBEntities _entities;
        private EventAggregator _eventAggregator;
        private IUnityContainer _container;

        public ProjectServiceProvider(DBEntities entities,
                                        EventAggregator eventAggregator,
                                        IUnityContainer container)
        {
            _container = container;
            _entities = entities;
            _eventAggregator = eventAggregator;

            _eventAggregator.GetEvent<ProjectCreationRequested>().Subscribe(
                () => CreateNewProject());
        }

        public bool AlterProjectInfo(Project target)
        {
            Views.ModifyProjectDetailsDialog modificationDialog = _container.Resolve<Views.ModifyProjectDetailsDialog>();
            modificationDialog.ProjectInstance = target;

            return (bool)modificationDialog.ShowDialog();
        }

        private Project CreateNewProject(Person leader = null)
        {
            Views.ProjectCreationDialog creationDialog = new Views.ProjectCreationDialog();

            if (creationDialog.ShowDialog() == true)
            {
                _eventAggregator.GetEvent<ProjectListUpdateRequested>().Publish();
                return creationDialog.ProjectInstance;
            }

            else
                return null;
        }
    }
}
