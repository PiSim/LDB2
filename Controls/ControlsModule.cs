using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using System;

namespace Controls
{
    public class ControlsModule : IModule
    {
        IRegionManager _regionManager;
        IUnityContainer _container;

        public ControlsModule(IRegionManager regionManager, IUnityContainer container)
        {
            _container = container;
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            _container.RegisterType<Views.MaterialCreationDialog>();
            _regionManager.RegisterViewWithRegion("ToolbarRegion", typeof(Views.ToolbarView));
        }
    }
}