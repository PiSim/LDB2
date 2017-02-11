using Microsoft.Practices.Unity;
using Prism.Events;
using Prism.Modularity;
using Prism.Regions;
using System;

namespace DBManager
{
    public class DBManagerModule : IModule
    {
        IEventAggregator _eventAggregator;
        IUnityContainer _container;

        public DBManagerModule(IEventAggregator eventAggregator, IUnityContainer container)
        {
            _eventAggregator = eventAggregator;
            _container = container;
        }

        public void Initialize()
        {
            _container.RegisterType<DBEntities>(new ContainerControlledLifetimeManager());
            
        }
    }
}