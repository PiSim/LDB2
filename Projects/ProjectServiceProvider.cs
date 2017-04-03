using DBManager;
using Infrastructure;
using Microsoft.Practices.Unity;
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
        private IUnityContainer _container;

        public ProjectServiceProvider(DBEntities entities,
                                        IUnityContainer container)
        {
            _container = container;
            _entities = entities;
        }

        public bool AlterProjectInfo(Project target)
        {
            Views.ModifyProjectDetailsDialog modificationDialog = _container.Resolve<Views.ModifyProjectDetailsDialog>();
            modificationDialog.ProjectInstance = target;

            return (bool)modificationDialog.ShowDialog();
        }

        public Project CreateNewProject(Person leader = null)
        {
            Views.ProjectCreationDialog creationDialog = new Views.ProjectCreationDialog(_entities);

            if (creationDialog.ShowDialog() == true)
            {
                return creationDialog.ProjectInstance;
            }

            else
                return null;
        }
    }
}
