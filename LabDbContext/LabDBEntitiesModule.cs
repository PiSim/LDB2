using DataAccess;
using Prism.Ioc;
using Prism.Modularity;
using System.Data.Entity.Infrastructure;

namespace LabDbContext
{
    public class LabDbEntitiesModule : IModule
    {
        #region Constructors

        public LabDbEntitiesModule()
        {
        }

        #endregion Constructors

        #region Methods

        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry registry)
        {
            registry.RegisterSingleton<IDbContextFactory<LabDbEntities>, LabDBContextFactory>();
            registry.Register<IDataService<LabDbEntities>, LabDbData>();
            registry.Register<IDataService, DataAccessService>();
        }

        #endregion Methods
    }
}