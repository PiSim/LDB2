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
        private DelegateCommand _newProject, _openProject;
        private EventAggregator _eventAggregator;
        private Project _selectedProject;

        public ProjectMainViewModel(DBPrincipal principal,
                                    EventAggregator aggregator) 
            : base()
        {
            _eventAggregator = aggregator;

            _eventAggregator.GetEvent<ProjectListUpdateRequested>().Subscribe(
                () => RaisePropertyChanged("ProjectList"));

            _newProject = new DelegateCommand(
                () =>
                {
                    _eventAggregator.GetEvent<ProjectCreationRequested>().Publish();
                });

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
            get { return ProjectService.GetProjects(); } 
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
