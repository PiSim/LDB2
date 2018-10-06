using Prism.Ioc;
using Prism.Modularity;

namespace Infrastructure
{
    [Module(ModuleName = "InfrastructureModule")]
    public class InfrastructureModule : IModule
    {
        #region Constructors

        public InfrastructureModule()
        {
        }

        #endregion Constructors

        #region Methods

        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
        }

        #endregion Methods
    }
}