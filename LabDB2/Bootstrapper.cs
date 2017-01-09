using Microsoft.Practices.Unity;
using Prism.Unity;
using LabDB2.Views;
using System;
using System.Windows;

namespace LabDB2
{
    class Bootstrapper : UnityBootstrapper
    {

        protected override void ConfigureModuleCatalog()
        {
            Type BatchesModuleType = typeof(Batches.BatchesModule);
            ModuleCatalog.AddModule(
                new Prism.Modularity.ModuleInfo(BatchesModuleType.Name, BatchesModuleType.AssemblyQualifiedName));

            Type DBManagerModuleType = typeof(DBManager.DBManagerModule);
            ModuleCatalog.AddModule(
                new Prism.Modularity.ModuleInfo()
                {
                    ModuleName = DBManagerModuleType.Name,
                    ModuleType = DBManagerModuleType.AssemblyQualifiedName
                });

            DBManagerModuleType = typeof(Infrastructure.InfrastructureModule);
            ModuleCatalog.AddModule(
                new Prism.Modularity.ModuleInfo()
                {
                    ModuleName = DBManagerModuleType.Name,
                    ModuleType = DBManagerModuleType.AssemblyQualifiedName
                });

            DBManagerModuleType = typeof(Navigation.NavigationModule);
            ModuleCatalog.AddModule(
                new Prism.Modularity.ModuleInfo()
                {
                    ModuleName = DBManagerModuleType.Name,
                    ModuleType = DBManagerModuleType.AssemblyQualifiedName
                });

            Type ControlsModuleType = typeof(Controls.ControlsModule);
            ModuleCatalog.AddModule(
                new Prism.Modularity.ModuleInfo(ControlsModuleType.Name, ControlsModuleType.AssemblyQualifiedName));
        }

        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<Shell>();
        }

        protected override void InitializeShell()
        {
            Application.Current.MainWindow.Show();
        }
    }
}
