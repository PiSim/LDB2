using Prism.Ioc;
using Prism.Modularity;
using System;

namespace Controls
{
    public class ControlsModule : IModule
    {
        #region Constructors

        public ControlsModule()
        {
        }

        #endregion Constructors

        #region Methods

        public void OnInitialized(IContainerProvider containerProvider)
        {
            throw new NotImplementedException();
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<Views.StringInputDialog>();
            containerRegistry.Register<ViewModels.BatchPickerDialogViewModel>();
            containerRegistry.Register<ViewModels.MaterialCreationDialogViewModel>();
            containerRegistry.Register<ViewModels.StatusBarViewModel>();
            containerRegistry.Register<ViewModels.ToolbarViewModel>();
        }

        #endregion Methods
    }
}