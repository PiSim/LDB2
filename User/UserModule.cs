using Infrastructure;
using Prism.Ioc;
using Prism.Modularity;
using System;

namespace User
{
    [Module(ModuleName = "UserModule")]
    public class UserModule : IModule
    {
        #region Constructors

        public UserModule()
        {
        }

        #endregion Constructors

        #region Methods

        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<Object, Views.CurrentUserMain>(UserViewNames.CurrentUserMain);
            containerRegistry.Register<ViewModels.CurrentUserMainViewModel>();
        }

        #endregion Methods
    }
}