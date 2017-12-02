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
    public class ProjectService
    {
        private DBEntities _entities;
        private EventAggregator _eventAggregator;
        private IUnityContainer _container;

        public ProjectService(DBEntities entities,
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



        internal static void UpdateProjectCosts()
        {
            IEnumerable<Project> _prjList = ProjectService.GetProjects();

            foreach (Project prj in _prjList)
            {
                double oldvalue = prj.TotalExternalCost;

                prj.UpdateExternalReportCost();

                if (prj.TotalExternalCost != oldvalue)
                    prj.Update();
            }
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
