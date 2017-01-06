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
            Type DBManagerModuleType = typeof(DBManager.DBManagerModule);
            ModuleCatalog.AddModule(
                new Prism.Modularity.ModuleInfo()
                {
                    ModuleName = DBManagerModuleType.Name,
                    ModuleType = DBManagerModuleType.AssemblyQualifiedName
                });
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
