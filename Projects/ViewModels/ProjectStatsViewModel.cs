using DataAccess;
using Infrastructure;
using Infrastructure.Events;
using Infrastructure.Queries;
using LabDbContext;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Projects.ViewModels
{
    public class ProjectStatsViewModel : BindableBase
    {
        #region Fields

        private readonly IDataService<LabDbEntities> _labDbData;
        private readonly IProjectService _projectService;
        private IEventAggregator _eventAggregator;

        #endregion Fields

        #region Constructors

        public ProjectStatsViewModel(IEventAggregator eventAggregator,
                                    IDataService<LabDbEntities> labDbData,
                                    IProjectService projectService)
        {
            _labDbData = labDbData;
            _eventAggregator = eventAggregator;
            _projectService = projectService;

            UpdateProjectStats = new DelegateCommand(
                () =>
                {
                    _projectService.UpdateAllCosts();
                    RaisePropertyChanged("ProjectStatList");
                });

            _eventAggregator.GetEvent<ProjectChanged>()
                            .Subscribe(
                            ect =>
                            {
                                RaisePropertyChanged("ProjectStatList");
                            });
        }

        #endregion Constructors

        #region Properties

        public bool IsAdmin => Thread.CurrentPrincipal.IsInRole(UserRoleNames.Admin);

        public IEnumerable<Project> ProjectStatList => _labDbData.RunQuery(new ProjectsQuery())
                                                                .ToList();

        public DelegateCommand UpdateProjectStats { get; }

        #endregion Properties
    }
}