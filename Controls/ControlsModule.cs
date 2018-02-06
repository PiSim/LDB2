using Infrastructure;
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
            _container.RegisterType<Views.StringInputDialog>();

            _container.RegisterType<ViewModels.BatchPickerDialogViewModel>();
            _container.RegisterType<ViewModels.ColorPickerDialogViewModel>();
            _container.RegisterType<ViewModels.MaterialCreationDialogViewModel>();
            _container.RegisterType<ViewModels.ProjectPickerDialogViewModel>();
            _container.RegisterType<ViewModels.StatusBarViewModel>();
            _container.RegisterType<ViewModels.ToolbarViewModel>();

            _regionManager.RegisterViewWithRegion(RegionNames.CalibrationEditFileListRegion,
                                                typeof(Views.FileListControl));
            _regionManager.RegisterViewWithRegion(RegionNames.MethodFileRegion,
                                                typeof(Views.FileListControl));
            _regionManager.RegisterViewWithRegion(RegionNames.NewCalibrationFileListRegion, 
                                                typeof(Views.FileListControl));
            _regionManager.RegisterViewWithRegion(RegionNames.SpecificationEditFileRegion,
                                                typeof(Views.FileListControl));
            _regionManager.RegisterViewWithRegion(RegionNames.StandardFileListRegion,
                                                typeof(Views.FileListControl));
            _regionManager.RegisterViewWithRegion(RegionNames.ToolbarRegion, 
                                                typeof(Views.Toolbar));
            _regionManager.RegisterViewWithRegion(RegionNames.StatusbarRegion,
                                                typeof(Views.StatusBar)); 
        }
    }
}