using Controls.Views;
using Infrastructure;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System;

namespace Tasks
{
    public class TasksModule : IModule
    {
        #region Constructors

        public TasksModule()
        {
        }

        #endregion Constructors

        #region Methods

        public void Initialize()
        {
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
            IRegionManager regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion(RegionNames.MainNavigationRegion
                                                , typeof(Views.TaskNavigationItem));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<ITaskService, TaskService>();

            containerRegistry.Register<Object, Views.TaskEdit>(TaskViewNames.TaskEditView);
            containerRegistry.Register<Object, Views.TaskMain>(TaskViewNames.TaskMainView);

            containerRegistry.Register<Views.TaskCreationDialog>();
        }

        #endregion Methods
    }
}