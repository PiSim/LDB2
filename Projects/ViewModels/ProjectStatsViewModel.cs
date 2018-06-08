using DBManager;
using DBManager.Services;
using Infrastructure;
using Infrastructure.Events;
using Infrastructure.Wrappers;
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
    public class ProjectStatsViewModel : BindableBase
    {
        private DBPrincipal _principal;
        private DelegateCommand _updateStats;
        private EventAggregator _eventAggregator;
        private readonly IDataService _dataService;
        private readonly IProjectService _projectService;

        public ProjectStatsViewModel(DBPrincipal principal,
                                    EventAggregator eventAggregator,
                                    IDataService dataService,
                                    IProjectService projectService)
        {
            _dataService = dataService;
            _eventAggregator = eventAggregator;
            _principal = principal;
            _projectService = projectService;

            _updateStats = new DelegateCommand(
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

        public bool IsAdmin => _principal.IsInRole(UserRoleNames.Admin);

        public IEnumerable<Project> ProjectStatList => _dataService.GetProjects(true);

        public DelegateCommand UpdateProjectStats => _updateStats;
    }
}
