using DataAccess;
using Infrastructure;
using Infrastructure.Events;
using Infrastructure.Queries;
using LabDbContext;
using Prism.Events;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace Projects
{
    public class ProjectService : IProjectService
    {
        #region Fields

        private IDbContextFactory<LabDbEntities> _dbContextFactory;
        private IEventAggregator _eventAggregator;
        private IDataService<LabDbEntities> _labDbData;

        #endregion Fields

        #region Constructors

        public ProjectService(IDbContextFactory<LabDbEntities> dbContextFactory,
                                        IDataService<LabDbEntities> labDbData,
                            IEventAggregator eventAggregator)
        {
            _labDbData = labDbData;
            _dbContextFactory = dbContextFactory;
            _eventAggregator = eventAggregator;
        }

        #endregion Constructors

        #region Methods

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
            IEnumerable<Project> _prjList = _labDbData.RunQuery(new ProjectsQuery())
                                                                .ToList();

            foreach (Project prj in _prjList)
            {
                double oldExternalValue = prj.TotalExternalCost;
                double oldInternalValue = prj.TotalReportDuration;

                prj.TotalExternalCost = prj.GetExternalReportCost();
                prj.TotalReportDuration = prj.GetInternalReportCost();

                if (prj.TotalExternalCost != oldExternalValue
                    || prj.TotalReportDuration != oldInternalValue)
                    prj.Update();
            }
        }

        #endregion Methods
    }
}