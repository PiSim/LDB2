using Infrastructure;
using Prism.Modularity;
using Prism.Regions;
using System;
using Unity;
using Unity.Lifetime;

namespace Instruments
{
    [Module(ModuleName = "InstrumentsModule")]
    public class InstrumentsModule : IModule
    {
        IRegionManager _regionManager;
        IUnityContainer _container;

        public InstrumentsModule(RegionManager regionManager, 
                                IUnityContainer container)
        {
            _container = container;
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            DBPrincipal principal = _container.Resolve<DBPrincipal>();

            _container.RegisterType<Views.InstrumentCreationDialog>();

            _container.RegisterType <ViewModels.AddPropertyDialogViewModel>();
            _container.RegisterType<ViewModels.InstrumentCreationDialogViewModel>();
            _container.RegisterType<ViewModels.InstrumentEditViewModel>();
            _container.RegisterType<ViewModels.InstrumentMainViewModel>();

            _container.RegisterType<Object, Views.CalibrationReportEdit>(InstrumentViewNames.CalibrationReportEditView);
            _container.RegisterType<Object, Views.InstrumentEdit>(InstrumentViewNames.InstrumentEditView);
            _container.RegisterType<Object, Views.InstrumentMain>(InstrumentViewNames.InstrumentsMainView);

            _regionManager.RegisterViewWithRegion(RegionNames.InstrumentEditMetrologyRegion,
                                                    typeof(Views.Metrology));

            if (principal.IsInRole(UserRoleNames.InstrumentView))
                _regionManager.RegisterViewWithRegion(RegionNames.MainNavigationRegion, 
                                                    typeof(Views.InstrumentsNavigationItem));

            _container.RegisterType<IInstrumentService, InstrumentService>
                (new ContainerControlledLifetimeManager());
            _container.Resolve<IInstrumentService>();

        }
    }
}