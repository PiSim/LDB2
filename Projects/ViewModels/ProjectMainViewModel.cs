using DBManager;
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
        private DBEntities _entities;
        private DelegateCommand _newProject, _openProject;
        private EventAggregator _eventAggregator;
        private IProjectServiceProvider _projectServiceProvider;
        private Project _selectedProject;

        public ProjectMainViewModel(DBEntities entities, 
                                    EventAggregator aggregator, 
                                    IProjectServiceProvider projectServiceProvider) 
            : base()
        {
            _entities = entities;
            _eventAggregator = aggregator;
            _projectServiceProvider = projectServiceProvider;

            _eventAggregator.GetEvent<ProjectListUpdateRequested>().Subscribe(() => OnPropertyChanged("ProjectList"));

            _newProject = new DelegateCommand(
                () =>
                {
                    Project tempProject = _projectServiceProvider.CreateNewProject();
                    if (tempProject != null)
                    {
                        OnPropertyChanged("ProjectList");
                    }
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
        
        public List<Project> ProjectList
        {
            get { return new List<Project>(_entities.Projects); } 
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
