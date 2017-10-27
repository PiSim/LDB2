using DBManager;
using DBManager.Services;
using Infrastructure;
using Infrastructure.Events;
using Infrastructure.Wrappers;
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
        private EventAggregator _eventAggregator;

        public ProjectStatsViewModel(EventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;

            _eventAggregator.GetEvent<ProjectListUpdateRequested>()
                            .Subscribe(
                            () =>
                            {
                                RaisePropertyChanged("ProjectStatList");
                            });
        }

        public IEnumerable<ProjectStatsWrapper> ProjectStatList
        {
            get
            {
                return ProjectService.GetProjects()
                                    .Select(prj => new ProjectStatsWrapper(prj));
            }
        }
    }
}
