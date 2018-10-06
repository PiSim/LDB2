using Controls.Views;
using Infrastructure;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Regions;
using System;

namespace Materials
{
    [Module(ModuleName = "MaterialsModule")]
    public class MaterialsModule : IModule
    {
        #region Constructors

        public MaterialsModule()
        {
        }

        #endregion Constructors

        #region Methods

        public void OnInitialized(IContainerProvider containerProvider)
        {
            IRegionManager regionManager = containerProvider.Resolve<IRegionManager>();
            regionManager.RegisterViewWithRegion(RegionNames.MainNavigationRegion,
                                                typeof(Views.BatchesNavigationItem));
            regionManager.RegisterViewWithRegion(RegionNames.MainNavigationRegion,
                                                typeof(Views.MaterialsNavigationItem));
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<Object, Views.AspectDetail>(MaterialViewNames.AspectDetail);
            containerRegistry.Register<Object, Views.BatchInfo>(MaterialViewNames.BatchInfoView);
            containerRegistry.Register<Object, Views.BatchMain>(MaterialViewNames.BatchesView);
            containerRegistry.Register<Object, Views.ColourEdit>(MaterialViewNames.ColourEdit);
            containerRegistry.Register<Object, Views.ColourMain>(MaterialViewNames.ColourMain);
            containerRegistry.Register<Object, Views.MaterialDetail>(MaterialViewNames.MaterialDetail);
            containerRegistry.Register<Object, Views.ExternalConstructionDetail>(MaterialViewNames.ExternalConstructionDetail);
            containerRegistry.Register<Object, Views.MaterialInfoMain>(MaterialViewNames.MaterialView);
            containerRegistry.RegisterSingleton<MaterialService>();
        }

        #endregion Methods
    }
}