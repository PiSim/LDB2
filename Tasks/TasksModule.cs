using Infrastructure;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using System;
using Unity;
using Unity.Lifetime;

namespace Tasks
{
    public class TasksModule : IModule
    {
        IRegionManager _regionManager;
        IUnityContainer _container;

        public TasksModule(IRegionManager regionManager, IUnityContainer container)
        {
            _container = container;
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            _container.RegisterType<ITaskService, TaskService>();

            _container.RegisterType<Object, Views.TaskEdit>(TaskViewNames.TaskEditView);
            _container.RegisterType<Object, Views.TaskMain>(TaskViewNames.TaskMainView);
            
            _container.RegisterType<ViewModels.TaskEditViewModel>();
            _container.RegisterType<ViewModels.TaskMainViewModel>();
            
            _container.RegisterType<Views.TaskCreationDialog>();

            _container.RegisterType<TaskService>(new ContainerControlledLifetimeManager());
            _container.Resolve<TaskService>();
            
            _regionManager.RegisterViewWithRegion(RegionNames.MainNavigationRegion
                                                , typeof(Views.TaskNavigationItem));
        }
    }
}