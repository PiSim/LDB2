using Controls.Views;
using Infrastructure;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Threading;

namespace Projects
{
    public class ProjectsModule : IModule
    {
        #region Constructors

        public ProjectsModule()
        {
        }

        #endregion Constructors

        #region Methods

        public void OnInitialized(IContainerProvider containerProvider)
        {
            IRegionManager regionManager = containerProvider.Resolve<IRegionManager>();
            if (Thread.CurrentPrincipal.IsInRole(UserRoleNames.ProjectView))
                regionManager.RegisterViewWithRegion(RegionNames.MainNavigationRegion,
                                                    typeof(Views.ProjectsNavigationItem));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IProjectService, ProjectService>();

            containerRegistry.Register<Views.ProjectCreationDialog>();
            containerRegistry.Register<Object, Views.ProjectMain>(ProjectsViewNames.ProjectMainView);
            containerRegistry.Register<Object, Views.ProjectInfo>(ProjectsViewNames.ProjectInfoView);
        }

        #endregion Methods
    }
}