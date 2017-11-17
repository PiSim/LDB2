using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using System;

namespace Reporting
{
    [Module(ModuleName = "Reporting")]
    public class ReportingModule : IModule
    {
        IRegionManager _regionManager;
        private IUnityContainer _container;

        public ReportingModule(IRegionManager regionManager,
                                IUnityContainer container)
        {
            _container = container;
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            _container.RegisterType<ReportingEngine>(new ContainerControlledLifetimeManager());
            _container.Resolve<ReportingEngine>();
            _container.RegisterType<IReportingService, ReportingService>();
        }
    }
}