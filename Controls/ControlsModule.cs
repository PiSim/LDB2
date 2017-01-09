using Prism.Modularity;
using Prism.Regions;
using System;

namespace Controls
{
    public class ControlsModule : IModule
    {
        IRegionManager _regionManager;

        public ControlsModule(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            _regionManager.RegisterViewWithRegion("ToolbarRegion", typeof(Views.ToolbarView));
        }
    }
}