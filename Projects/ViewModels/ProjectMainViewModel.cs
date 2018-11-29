using Controls.Views;
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
    public class ProjectMainViewModel : BindableBase
    {
        #region Fields

        private IEventAggregator _eventAggregator;
        private IDataService<LabDbEntities> _labDbData;
        private IProjectService _projectService;
        private Project _selectedProject;

        #endregion Fields

        #region Constructors

        public ProjectMainViewModel(IDataService<LabDbEntities> labDbData,
                                    IEventAggregator aggregator,
                                    IProjectService projectService)
            : base()
        {
            _labDbData = labDbData;
            _projectService = projectService;
            _eventAggregator = aggregator;

            _eventAggregator.GetEvent<ProjectChanged>()
                            .Subscribe(ect => RaisePropertyChanged("ProjectList"));

            NewProjectCommand = new DelegateCommand(
                () =>
                {
                    _projectService.CreateProject();
                },
                () => IsProjectEdit);

            OpenProjectCommand = new DelegateCommand(
                () =>
                {
                    NavigationToken token = new NavigationToken(ProjectsViewNames.ProjectInfoView,
                                                                _selectedProject);
                    _eventAggregator.GetEvent<NavigationRequested>().Publish(token);
                },
                () => _selectedProject != null);
        }

        #endregion Constructors

        #region Properties

        public bool IsProjectEdit => Thread.CurrentPrincipal.IsInRole(UserRoleNames.ProjectEdit);

        public DelegateCommand NewProjectCommand { get; }

        public DelegateCommand OpenProjectCommand { get; }

        public IEnumerable<Project> ProjectList => _labDbData.RunQuery(new ProjectsQuery())
                                                                .ToList();

        public string ProjectStatRegionName => RegionNames.ProjectStatRegion;

        public Project SelectedProject
        {
            get { return _selectedProject; }
            set
            {
                _selectedProject = value;
                OpenProjectCommand.RaiseCanExecuteChanged();
            }
        }

        #endregion Properties
    }
}