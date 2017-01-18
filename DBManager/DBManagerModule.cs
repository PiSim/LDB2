using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using System;

namespace DBManager
{
    public class DBManagerModule : IModule
    {
        IUnityContainer _container;

        public DBManagerModule(IUnityContainer container)
        {
            _container = container;
        }

        public void Initialize()
        {
            _container.RegisterType<DBEntities>(new ContainerControlledLifetimeManager());
        }
    }
}