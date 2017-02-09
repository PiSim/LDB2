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
            _container.RegisterType<Views.ColorPickerDialog>();
            _container.RegisterType<Views.MaterialCreationDialog>();
            _container.RegisterType<Views.ProjectPickerDialog>();
            _container.RegisterType<Views.ReportCreationDialog>();
            _container.RegisterType<Views.TaskCreationDialog>();
            _regionManager.RegisterViewWithRegion("ToolbarRegion", typeof(Views.ToolbarView));
        }
    }
}