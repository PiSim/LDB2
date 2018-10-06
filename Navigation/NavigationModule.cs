using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using System;

namespace Navigation
{
    public class NavigationModule : IModule
    {
        private IUnityContainer _container;

        public NavigationModule(IUnityContainer container)
        {
            _container = container;
        }

        public void Initialize()
        {
        }
    }
}