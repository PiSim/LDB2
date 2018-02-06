using Microsoft.Practices.Unity;
using Prism.Events;
using Prism.Modularity;
using Prism.Regions;
using Infrastructure.Queries;
using System;
using Unity;

namespace Infrastructure
{
    [Module(ModuleName = "InfrastructureModule")]
    public class InfrastructureModule : IModule
    {
        IRegionManager _regionManager;
        IUnityContainer _container;

        public InfrastructureModule(IUnityContainer container,
                                    RegionManager regionManager)
        {
            _container = container;
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            _container.RegisterType<ArrivedUntestedBatchesQuery>();
        }
    }
}