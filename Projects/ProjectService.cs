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
    public class ProjectService : IProjectService
    {
        private DBEntities _entities;
        private EventAggregator _eventAggregator;
        private IDataService _dataService;
        private IUnityContainer _container;

        public ProjectService(DBEntities entities,
                                EventAggregator eventAggregator,
                                IDataService dataService,
                                IUnityContainer container)
        {
            _container = container;
            _dataService = dataService;
            _entities = entities;
            _eventAggregator = eventAggregator;
            
        }
        
        public Project CreateProject()
        {
            Views.ProjectCreationDialog creationDialog = new Views.ProjectCreationDialog();

            if (creationDialog.ShowDialog() == true)
            {
                _eventAggregator.GetEvent<ProjectChanged>()
                                .Publish(new EntityChangedToken(creationDialog.ProjectInstance,
                                                                EntityChangedToken.EntityChangedAction.Created));
                return creationDialog.ProjectInstance;
            }

            else
                return null;
        }
        
        /// <summary>
        /// Updates the ExternalCost of every project.
        /// TBD: Updates the internal cost too
        /// </summary>
        public void UpdateAllCosts()
        {
            IEnumerable<Project> _prjList = _dataService.GetProjects();

            foreach (Project prj in _prjList)
            {
                double oldExternalValue = prj.TotalExternalCost;
                double oldInternalValue = prj.TotalReportDuration;

                prj.TotalExternalCost = prj.GetExternalReportCost();
                prj.TotalReportDuration = prj.GetInternalReportCost();

                if (prj.TotalExternalCost != oldExternalValue
                    || prj.TotalReportDuration != oldInternalValue )
                    prj.Update();
            }
        }
    }
}
