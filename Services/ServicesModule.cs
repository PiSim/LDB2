using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using System;

namespace Services
{
    public class ServicesModule : IModule
    {
        IRegionManager _regionManager;
        IUnityContainer _container;

        public ServicesModule(IRegionManager regionManager,
                                IUnityContainer container)
        {
            _container = container;
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            _container.RegisterType<EventManager>(new ContainerControlledLifetimeManager());
            _container.Resolve<EventManager>();

            _container.RegisterType<ViewModels.AddTestDialogViewModel>();
            _container.RegisterType<ViewModels.BatchCreationDialogViewModel>();
            _container.RegisterType<ViewModels.MaintenanceEventCreationDialogViewModel>();
            _container.RegisterType<ViewModels.ReportCreationDialogViewModel>();
            _container.RegisterType<ViewModels.SampleLogDialogViewModel>();
        }
    }
}