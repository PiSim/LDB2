using Prism.Modularity;
using System;
using Prism.Ioc;
using DataAccess;

namespace LabDbContextCore
{
    public class LabDbContextCoreModule : IModule
    {
        public LabDbContextCoreModule()
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
            containerRegistry.Register<LabDbCore>();
        }
    }
}