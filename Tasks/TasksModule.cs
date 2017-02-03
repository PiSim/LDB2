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
            _container.RegisterType<Object, Views.TaskMainView>(ViewNames.TaskMainView);
            
            _regionManager.RegisterViewWithRegion(Navigation.RegionNames.MainNavigationRegion
                                                , typeof(Views.TaskNavigationItem));
        }
    }
}