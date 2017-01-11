using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using System;

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
            
            _regionManager.RegisterViewWithRegion("MainNavigationRegion", typeof(Views.TaskNavigationItem));
        }
    }
}