using DBManager;
using DBManager.Services;
using Infrastructure;
using Infrastructure.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projects.ViewModels
{
    public class ProjectMainViewModel : BindableBase
    {
        private DBPrincipal _principal;
        private DelegateCommand _newProject, _openProject;
        private EventAggregator _eventAggregator;
        private IDataService _dataService;
        private IProjectService _projectService;
        private Project _selectedProject;

        public ProjectMainViewModel(DBPrincipal principal,
                                    EventAggregator aggregator,
                                    IDataService dataService,
                                    IProjectService projectService) 
            : base()
        {
            _dataService = dataService;
            _projectService = projectService;
            _eventAggregator = aggregator;
            _principal = principal;

            _eventAggregator.GetEvent<ProjectChanged>()
                            .Subscribe(ect => RaisePropertyChanged("ProjectList"));

            _newProject = new DelegateCommand(
                () =>
                {
                    _projectService.CreateProject();
                },
                () => IsProjectEdit);

            _openProject = new DelegateCommand(
                () => 
                {
                    NavigationToken token = new NavigationToken(ProjectsViewNames.ProjectInfoView,
                                                                _selectedProject);
                    _eventAggregator.GetEvent<NavigationRequested>().Publish(token);
                },
                () => _selectedProject != null
            );
        }

        public bool IsProjectEdit
        {
            get { return _principal.IsInRole(UserRoleNames.ProjectEdit); }
        }

        public DelegateCommand NewProjectCommand
        {
            get { return _newProject; }
        }
        
        public DelegateCommand OpenProjectCommand
        {
            get { return _openProject; }
        }
        
        public IEnumerable<Project> ProjectList
        {
            get { return _dataService.GetProjects(); } 
        }

        public string ProjectStatRegionName
        {
            get { return RegionNames.ProjectStatRegion; }
        }
        
        public Project SelectedProject
        {
            get { return _selectedProject; }
            set
            {
                _selectedProject = value;
                _openProject.RaiseCanExecuteChanged();
            }
        }
    }
}
