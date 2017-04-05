﻿using Microsoft.Practices.Unity;
using Infrastructure;
using Prism.Modularity;
using Prism.Regions;
using System;

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
            _container.RegisterType<Views.NewCalibrationDialog>();
            _container.RegisterType<Views.NewMaintenanceEventDialog>();

            _container.RegisterType<ViewModels.InstrumentEditViewModel>();
            _container.RegisterType<ViewModels.InstrumentMainViewModel>();

            _container.RegisterType<Object, Views.InstrumentEdit>(ViewNames.InstrumentEditView);
            _container.RegisterType<Object, Views.InstrumentMain>(ViewNames.InstrumentsMainView);
            
            _regionManager.RegisterViewWithRegion(RegionNames.MainNavigationRegion, 
                                                typeof(Views.InstrumentsNavigationItem));

            _container.RegisterType<IInstrumentServiceProvider, InstrumentServiceProvider>
                (new ContainerControlledLifetimeManager());
        }
    }
}