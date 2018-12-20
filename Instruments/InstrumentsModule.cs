using Controls.Views;
using Infrastructure;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Threading;

namespace Instruments
{
    [Module(ModuleName = "InstrumentsModule")]
    public class InstrumentsModule : IModule
    {
        #region Constructors

        public InstrumentsModule()
        {
        }

        #endregion Constructors

        #region Methods

        public void OnInitialized(IContainerProvider containerProvider)
        {
            IRegionManager regionManager = containerProvider.Resolve<IRegionManager>();

            if (Thread.CurrentPrincipal.IsInRole(UserRoleNames.InstrumentView))
                regionManager.RegisterViewWithRegion(RegionNames.MainNavigationRegion,
                                                    typeof(Views.InstrumentsNavigationItem));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<Views.InstrumentCreationDialog>();

            containerRegistry.Register<ViewModels.InstrumentCreationDialogViewModel>();
            containerRegistry.Register<ViewModels.InstrumentEditViewModel>();
            containerRegistry.Register<ViewModels.InstrumentMainViewModel>();

            containerRegistry.Register<Object, Views.CalibrationReportEdit>(InstrumentViewNames.CalibrationReportEditView);
            containerRegistry.Register<Object, Views.InstrumentEdit>(InstrumentViewNames.InstrumentEditView);
            containerRegistry.Register<Object, Views.InstrumentMain>(InstrumentViewNames.InstrumentsMainView);

            containerRegistry.RegisterForNavigation(typeof(Views.InstrumentMain), InstrumentViewNames.InstrumentsMainView);

            containerRegistry.RegisterSingleton<InstrumentService, InstrumentService>();
        }

        #endregion Methods
    }
}