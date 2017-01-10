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
        LabDBEntities _entities;
        IEventAggregator _eventAggregator;
        IUnityContainer _container;
        IRegionManager _regionManager;

        public BatchesModule(RegionManager regionManager, IUnityContainer container, 
            IEventAggregator eventAggregator, LabDBEntities entities)
        {
            _container = container;
            _entities = entities;
            _eventAggregator = eventAggregator;
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            _container.RegisterType<Object, Views.BatchesView>(BatchesViewNames.BatchesView);
            _container.RegisterType<Object, Views.BatchQueryView>(BatchesViewNames.BatchQueryView);
            _regionManager.RegisterViewWithRegion("MainNavigationRegion", typeof(Views.BatchesNavigationItem));
        }
    }
}