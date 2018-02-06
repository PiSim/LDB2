using Prism.Modularity;
using System;
using Unity;

namespace Security
{
    public class SecurityModule : IModule
    {
        IUnityContainer _container;

        public SecurityModule(IUnityContainer container)
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