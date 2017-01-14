using DBManager;
using Navigation;
using Microsoft.Practices.Unity;
using Prism.Events;
using Prism.Modularity;
using Prism.Regions;
using Prism.Unity;
using System;
using System.Data.Entity;

namespace Batches
{
    [Module(ModuleName = "BatchesModule")]
    public class BatchesModule : IModule
    {
        DBEntities _entities;
        IEventAggregator _eventAggregator;
        IUnityContainer _container;
        IRegionManager _regionManager;

        public BatchesModule(RegionManager regionManager, IUnityContainer container, 
            IEventAggregator eventAggregator, DBEntities entities)
        {
            _container = container;
            _entities = entities;
            _eventAggregator = eventAggregator;
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            _container.RegisterType<Object, Views.BatchInfoView>(ViewNames.BatchInfoView);
            _container.RegisterType<Object, Views.BatchMainView>(ViewNames.BatchesView);
            _container.RegisterType<Object, Views.BatchQueryView>(ViewNames.BatchQueryView);
            _container.RegisterType<Object, Views.SampleLogView>(ViewNames.SampleLogView);
            _regionManager.RegisterViewWithRegion("MainNavigationRegion", typeof(Views.BatchesNavigationItem));
        }
    }
}