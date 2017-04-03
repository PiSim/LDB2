using DBManager;
using Infrastructure;
using Infrastructure.Events;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.ObjectModel;
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
        private ObservableCollection<Project> _projectList;
        private Project _selectedProject;

        public ProjectMainViewModel(DBEntities entities, 
                                    EventAggregator aggregator, 
                                    IProjectServiceProvider projectServiceProvider) 
            : base()
        {
            _entities = entities;
            _eventAggregator = aggregator;
            _projectServiceProvider = projectServiceProvider;

            _newProject = new DelegateCommand(
                () =>
                {
                    Project tempProject = _projectServiceProvider.CreateNewProject();
                    _projectList.Add(tempProject);
                    SelectedProject = tempProject;
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
            
            _projectList = new ObservableCollection<Project>(_entities.Projects);
        }

        public DelegateCommand NewProjectCommand
        {
            get { return _newProject; }
        }
        
        public DelegateCommand OpenProjectCommand
        {
            get { return _openProject; }
        }
        
        public ObservableCollection<Project> ProjectList
        {
            get { return _projectList; } 
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
