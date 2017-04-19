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
            _container.RegisterType<Object, Views.BatchInfo>(MaterialViewNames.BatchInfoView);
            _container.RegisterType<Object, Views.BatchMain>(MaterialViewNames.BatchesView);
            _container.RegisterType<Object, Views.ColourEdit>(MaterialViewNames.ColourEdit);
            _container.RegisterType<Object, Views.ColourMain>(MaterialViewNames.ColourMain);
            _container.RegisterType<Object, Views.SampleLogView>(MaterialViewNames.SampleLogView);

            _container.RegisterType<Views.BatchPickerDialog>();
            _container.RegisterType<Views.ColorCreationDialog>();
            _container.RegisterType<Views.ColorPickerDialog>();
            _container.RegisterType<Views.MaterialCreationDialog>();
            _container.RegisterType<Views.ProjectPickerDialog>();
            _container.RegisterType<Views.SampleLogDialog>();

            _container.RegisterType<ViewModels.BatchInfoViewModel>();
            _container.RegisterType<ViewModels.BatchMainViewModel>();
            _container.RegisterType<ViewModels.BatchPickerDialogViewModel>();
            _container.RegisterType<ViewModels.ColorCreationDialogViewModel>();
            _container.RegisterType<ViewModels.ColourEditViewModel>();
            _container.RegisterType<ViewModels.ColourMainViewModel>();
            _container.RegisterType<ViewModels.SampleLogDialogViewModel>();

            _container.RegisterType<IMaterialServiceProvider, MaterialServiceProvider>(new ContainerControlledLifetimeManager());
            _container.Resolve<IMaterialServiceProvider>();

            _regionManager.RegisterViewWithRegion(RegionNames.ExternalReportBatchListRegion,
                                                typeof(Views.BatchList));
            _regionManager.RegisterViewWithRegion(RegionNames.ProjectBatchListRegion,
                                                typeof(Views.BatchList));

            _regionManager.RegisterViewWithRegion(RegionNames.MainNavigationRegion, 
                                                typeof(Views.BatchesNavigationItem));
            _regionManager.RegisterViewWithRegion(RegionNames.MainNavigationRegion,
                                                typeof(Views.ColoursNavigationItem));
        }
    }
}