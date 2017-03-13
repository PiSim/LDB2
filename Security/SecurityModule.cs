using Microsoft.Practices.Unity;
using Prism.Modularity;
using System;

namespace Security
{
    public class SecurityModule : IModule
    {
        UnityContainer _container;

        public SecurityModule(UnityContainer container)
        {
            _container = container;
        }

        public void Initialize()
        {
            _container.RegisterType<AuthenticationService>();
            _container.RegisterType<Views.LoginDialog>();
        }
    }
}