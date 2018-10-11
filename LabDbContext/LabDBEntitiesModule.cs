using DataAccess;
using Prism.Ioc;
using Prism.Modularity;
using System.Data.Entity.Infrastructure;

namespace LabDbContext
{
    [Module(ModuleName = "LabDbContext")]
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
            return;
        }

        public void RegisterTypes(IContainerRegistry registry)
        {
            LabDBContextFactory LDbContextFactory = new LabDBContextFactory("LabDb_PRD");
            registry.RegisterInstance(typeof(IDbContextFactory<LabDbEntities>), LDbContextFactory);
            registry.Register<IDataService<LabDbEntities>, LabDbData>();
        }

        #endregion Methods
    }
}