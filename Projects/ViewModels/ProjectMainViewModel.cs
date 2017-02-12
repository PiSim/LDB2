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
    internal class ProjectMainViewModel : BindableBase
    {
        private DBEntities _entities;
        private DelegateCommand _openProject;
        private EventAggregator _eventAggregator;
        private ObservableCollection<Project> _projectList;
        private Project _selectedProject;

        internal ProjectMainViewModel(DBEntities entities, EventAggregator aggregator) 
            : base()
        {
            _entities = entities;
            _eventAggregator = aggregator;
            _openProject = new DelegateCommand(
                () => 
                {
                    ObjectNavigationToken token = new ObjectNavigationToken(_selectedProject, ProjectsViewNames.ProjectInfoView);
                    _eventAggregator.GetEvent<VisualizeObjectRequested>().Publish(token);
                }
            );
            
            _projectList = new ObservableCollection<Project>(_entities.Projects);
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
            set { _selectedProject = value; }
        }
    }
}
