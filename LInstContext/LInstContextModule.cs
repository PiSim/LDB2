using DataAccessCore;
using Microsoft.EntityFrameworkCore.Design;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;

namespace LInst
{
    public class LInstModule : IModule
    {
        public void OnInitialized(IContainerProvider containerProvider)
        {
 
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton(typeof(IDesignTimeDbContextFactory<LInstContext>), typeof(LInstContextFactory));
            containerRegistry.RegisterSingleton(typeof(IDataService<LInstContext>), typeof(LInstData));            
        }
    }
}