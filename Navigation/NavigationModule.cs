﻿using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using System;
using Unity;
using Unity.Lifetime;

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
            _container.RegisterType<NavigationServiceProvider>(new ContainerControlledLifetimeManager());

            _container.Resolve<NavigationServiceProvider>();
        }
    }
}