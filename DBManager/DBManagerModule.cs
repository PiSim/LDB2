using Prism.Modularity;
using Prism.Regions;
using System;

namespace DBManager
{
    [Module(ModuleName = "DBManagerModule")]
    public class DBManagerModule : IModule
    {

        IRegionManager _regionManager;
         
        public DBManagerModule(RegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            throw new NotImplementedException();
        }
    }
}