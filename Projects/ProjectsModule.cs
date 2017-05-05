﻿using Infrastructure;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using System;

namespace Projects
{
    public class ProjectsModule : IModule
    {
        IRegionManager _regionManager;
        IUnityContainer _container;

        public ProjectsModule(IRegionManager regionManager, IUnityContainer container)
        {
            _container = container;
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            _container.RegisterType<Views.ModifyProjectDetailsDialog>();

            _container.RegisterType<Object, Views.ProjectMain>
                (ProjectsViewNames.ProjectMainView);
            _container.RegisterType<Object, Views.ProjectInfo>
                (ProjectsViewNames.ProjectInfoView);

            _container.RegisterType<ViewModels.ProjectInfoViewModel>();
            _container.RegisterType<ViewModels.ProjectMainViewModel>();

            _regionManager.RegisterViewWithRegion(RegionNames.BatchProjectDetailsRegion,
                                                typeof(Views.ProjectDetailsControl));
            _regionManager.RegisterViewWithRegion(RegionNames.ExternalReportProjectDetailsRegion,
                                                typeof(Views.ProjectDetailsControl));
            _regionManager.RegisterViewWithRegion(RegionNames.TaskEditProjectDetailsRegion,
                                                typeof(Views.ProjectDetailsControl));

            _regionManager.RegisterViewWithRegion(RegionNames.MainNavigationRegion, 
                                                typeof(Views.ProjectsNavigationItem));

            _container.RegisterType<IProjectServiceProvider, ProjectServiceProvider>
                (new ContainerControlledLifetimeManager());

            _container.Resolve<IProjectServiceProvider>();
        }
    }
}