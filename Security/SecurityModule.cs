using Prism.Ioc;
using Prism.Ioc;
using Prism.Modularity;
using System;

namespace Security
{
    public class SecurityModule : IModule
    {

        public SecurityModule()
        {
        }

        public void Initialize()
        {
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {

            containerRegistry.Register<Views.LoginDialog>();
        }
    }
}