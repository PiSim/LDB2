using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using System;
using System.Data.Entity;

namespace DBManager
{
    [Module(ModuleName = "DBManagerModule")]
    public class DBManagerModule : IModule
    {
        IUnityContainer _container;
         
        public DBManagerModule(IUnityContainer container)
        {
            _container = container;
        }

        public void Initialize()
        {

        }
    }
}