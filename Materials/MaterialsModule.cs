using Infrastructure;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Regions;
using System;

namespace Materials
{
    [Module(ModuleName = "MaterialsModule")]
    public class MaterialsModule : IModule
    {
        private IRegionManager _regionManager;
        private IUnityContainer _container;

        public MaterialsModule(RegionManager regionManager,
                                IUnityContainer container)
        {
            _container = container;
            _regionManager = regionManager;
        }

        public void Initialize()
        {
            _container.RegisterType<Object, Views.AspectDetail>(MaterialViewNames.AspectDetail);
            _container.RegisterType<Object, Views.BatchInfo>(MaterialViewNames.BatchInfoView);
            _container.RegisterType<Object, Views.BatchMain>(MaterialViewNames.BatchesView);
            _container.RegisterType<Object, Views.ColourEdit>(MaterialViewNames.ColourEdit);
            _container.RegisterType<Object, Views.ColourMain>(MaterialViewNames.ColourMain);
            _container.RegisterType<Object, Views.MaterialDetail>(MaterialViewNames.MaterialDetail);
            _container.RegisterType<Object, Views.ExternalConstructionDetail>(MaterialViewNames.ExternalConstructionDetail);
            _container.RegisterType<Object, Views.MaterialInfoMain>(MaterialViewNames.MaterialView);
            _container.RegisterType<Object, Views.SampleLog>(MaterialViewNames.SampleLogView);
            
            _container.RegisterType<Views.ColorCreationDialog>();

            _container.RegisterType<ViewModels.AspectDetailViewModel>();
            _container.RegisterType<ViewModels.AspectMainViewModel>();
            _container.RegisterType<ViewModels.BatchInfoViewModel>();
            _container.RegisterType<ViewModels.BatchMainViewModel>();
            _container.RegisterType<ViewModels.BatchStatusListViewModel>();
            _container.RegisterType<ViewModels.ColorCreationDialogViewModel>();
            _container.RegisterType<ViewModels.ColourEditViewModel>();
            _container.RegisterType<ViewModels.ColourMainViewModel>();
            _container.RegisterType<ViewModels.MaterialDetailViewModel>();
            _container.RegisterType<ViewModels.MaterialMainViewModel>();
            _container.RegisterType<ViewModels.ExternalConstructionDetailViewModel>();
            _container.RegisterType<ViewModels.ExternalConstructionMainViewModel>();
            _container.RegisterType<ViewModels.MaterialInfoMainViewModel>();
            _container.RegisterType<ViewModels.SampleArchiveViewModel>();
            _container.RegisterType<ViewModels.SampleLogViewModel>();

            _container.RegisterType<IMaterialService, MaterialService>();

            _regionManager.RegisterViewWithRegion(RegionNames.AspectDetailBatchListRegion,
                                                typeof(Views.BatchList));
            _regionManager.RegisterViewWithRegion(RegionNames.BatchStatusListRegion,
                                                typeof(Views.BatchStatusList));
            _regionManager.RegisterViewWithRegion(RegionNames.ColourEditBatchListRegion,
                                                typeof(Views.BatchList));
            _regionManager.RegisterViewWithRegion(RegionNames.MaterialDetailBatchListRegion,
                                                typeof(Views.BatchList));
            _regionManager.RegisterViewWithRegion(RegionNames.ExternalConstructionBatchListRegion,
                                                typeof(Views.BatchList));
            _regionManager.RegisterViewWithRegion(RegionNames.ExternalReportBatchListRegion,
                                                typeof(Views.BatchList));
            _regionManager.RegisterViewWithRegion(RegionNames.MaterialInfoAspectRegion,
                                                typeof(Views.AspectMain));
            _regionManager.RegisterViewWithRegion(RegionNames.MaterialInfoColourRegion,
                                                typeof(Views.ColourMain));
            _regionManager.RegisterViewWithRegion(RegionNames.MaterialInfoMaterialRegion,
                                                typeof(Views.MaterialMain));
            _regionManager.RegisterViewWithRegion(RegionNames.MaterialInfoExternalCostructionRegion,
                                                typeof(Views.ExternalConstructionMain));
            _regionManager.RegisterViewWithRegion(RegionNames.ProjectBatchListRegion,
                                                typeof(Views.BatchList));
            _regionManager.RegisterViewWithRegion(RegionNames.SampleArchiveRegion,
                                                typeof(Views.SampleArchive));
            _regionManager.RegisterViewWithRegion(RegionNames.SampleLongTermStorageRegion,
                                                typeof(Views.SampleLongTermStorage));

            _regionManager.RegisterViewWithRegion(RegionNames.MainNavigationRegion, 
                                                typeof(Views.BatchesNavigationItem));
            _regionManager.RegisterViewWithRegion(RegionNames.MainNavigationRegion,
                                                typeof(Views.MaterialsNavigationItem));
        }
    }
}