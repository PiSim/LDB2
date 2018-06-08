using Infrastructure;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using System;

namespace Projects
{
    public class ProjectsModule : IModule
    {
        DBPrincipal _principal;
        IRegionManager _regionManager;
        IUnityContainer _container;

        public ProjectsModule(DBPrincipal principal,
                            IRegionManager regionManager, 
                            IUnityContainer container)
        {
            _container = container;
            _regionManager = regionManager;
            _principal = principal;
        }

        public void Initialize()
        {
            _container.RegisterType<IProjectService, ProjectService>
                (new ContainerControlledLifetimeManager());

            _container.Resolve<IProjectService>();

            _container.RegisterType<Views.ProjectCreationDialog>();

            _container.RegisterType<Object, Views.ProjectMain>
                (ProjectsViewNames.ProjectMainView);
            _container.RegisterType<Object, Views.ProjectInfo>
                (ProjectsViewNames.ProjectInfoView);

            _container.RegisterType<ViewModels.ProjectCreationDialogViewModel>();
            _container.RegisterType<ViewModels.ProjectInfoViewModel>();
            _container.RegisterType<ViewModels.ProjectMainViewModel>();

            _regionManager.RegisterViewWithRegion(RegionNames.BatchProjectDetailsRegion,
                                                typeof(Views.ProjectDetailsControl));
            _regionManager.RegisterViewWithRegion(RegionNames.ExternalReportProjectDetailsRegion,
                                                typeof(Views.ProjectDetailsControl));
            _regionManager.RegisterViewWithRegion(RegionNames.ProjectExternalReportListRegion,
                                                typeof(Controls.Resources.ExternalReportList));
            _regionManager.RegisterViewWithRegion(RegionNames.ProjectStatRegion,
                                                typeof(Views.ProjectStats));
            _regionManager.RegisterViewWithRegion(RegionNames.TaskEditProjectDetailsRegion,
                                                typeof(Views.ProjectDetailsControl));

            if (_principal.IsInRole(UserRoleNames.ProjectView))
                _regionManager.RegisterViewWithRegion(RegionNames.MainNavigationRegion, 
                                                    typeof(Views.ProjectsNavigationItem));
        }
    }
}