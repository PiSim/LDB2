using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Unity;
using LabDB2.Views;
using Security.Views;
using System;
using System.Windows;

namespace LabDB2
{
    class Bootstrapper : UnityBootstrapper
    {
        protected override void ConfigureModuleCatalog()
        {

            // Loading common modules

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
                    ModuleType = DBManagerModuleType.AssemblyQualifiedName,
                });

            Type ControlsModuleType = typeof(Controls.ControlsModule);
            ModuleCatalog.AddModule(
                new Prism.Modularity.ModuleInfo()
                {
                    ModuleName = ControlsModuleType.Name,
                    ModuleType = ControlsModuleType.AssemblyQualifiedName
                });

            Type SecurityModuleType = typeof(Security.SecurityModule);
            ModuleCatalog.AddModule(
                new Prism.Modularity.ModuleInfo()
                {
                    ModuleName = SecurityModuleType.Name,
                    ModuleType = SecurityModuleType.AssemblyQualifiedName
                });

            // Initializing modules to be loaded on demand

            Type AdminModuleType = typeof(Admin.AdminModule);
            ModuleCatalog.AddModule(
                new Prism.Modularity.ModuleInfo()
                {
                    ModuleName = AdminModuleType.Name,
                    ModuleType = AdminModuleType.AssemblyQualifiedName,
                    InitializationMode = Prism.Modularity.InitializationMode.OnDemand
                });

            Type BatchesModuleType = typeof(Batches.BatchesModule);
            ModuleCatalog.AddModule(
                new Prism.Modularity.ModuleInfo()
                {
                    ModuleName = BatchesModuleType.Name,
                    ModuleType = BatchesModuleType.AssemblyQualifiedName,
                    InitializationMode = Prism.Modularity.InitializationMode.OnDemand
                });

            Type MastersModuleType = typeof(Masters.MastersModule);
            ModuleCatalog.AddModule(
                new Prism.Modularity.ModuleInfo()
                {
                    ModuleName = MastersModuleType.Name,
                    ModuleType = MastersModuleType.AssemblyQualifiedName,
                    InitializationMode = Prism.Modularity.InitializationMode.OnDemand
                });

            Type MethodsModuleType = typeof(Methods.MethodsModule);
            ModuleCatalog.AddModule(
                new Prism.Modularity.ModuleInfo()
                {
                    ModuleName = MethodsModuleType.Name,
                    ModuleType = MethodsModuleType.AssemblyQualifiedName,
                    InitializationMode = Prism.Modularity.InitializationMode.OnDemand
                });

            Type ProjectsModuleType = typeof(Projects.ProjectsModule);
            ModuleCatalog.AddModule(
                new Prism.Modularity.ModuleInfo()
                {
                    ModuleName = ProjectsModuleType.Name,
                    ModuleType = ProjectsModuleType.AssemblyQualifiedName,
                    InitializationMode = Prism.Modularity.InitializationMode.OnDemand
                });

            Type ReportsModuleType = typeof(Reports.ReportsModule);
            ModuleCatalog.AddModule(
                new Prism.Modularity.ModuleInfo()
                {
                    ModuleName = ReportsModuleType.Name,
                    ModuleType = ReportsModuleType.AssemblyQualifiedName,
                    InitializationMode = Prism.Modularity.InitializationMode.OnDemand
                });

            Type SpecificationModuleType = typeof(Specifications.SpecificationsModule);
            ModuleCatalog.AddModule(
                new Prism.Modularity.ModuleInfo()
                {
                    ModuleName = SpecificationModuleType.Name,
                    ModuleType = SpecificationModuleType.AssemblyQualifiedName,
                    InitializationMode = Prism.Modularity.InitializationMode.OnDemand
                });

            Type StandardModuleType = typeof(Standards.StandardsModule);
            ModuleCatalog.AddModule(
                new Prism.Modularity.ModuleInfo()
                {
                    ModuleName = StandardModuleType.Name,
                    ModuleType = StandardModuleType.AssemblyQualifiedName,
                    InitializationMode = Prism.Modularity.InitializationMode.OnDemand
                });

            Type TaskModuleType = typeof(Tasks.TasksModule);
            ModuleCatalog.AddModule(
                new Prism.Modularity.ModuleInfo()
                {
                    ModuleName = TaskModuleType.Name,
                    ModuleType = TaskModuleType.AssemblyQualifiedName,
                    InitializationMode = Prism.Modularity.InitializationMode.OnDemand
                });
        }

        protected override DependencyObject CreateShell()
        {
            return Container.Resolve<Shell>();
        }

        protected override void InitializeShell()
        {
            LoginDialog logger = Container.Resolve<LoginDialog>();

            if (logger.ShowDialog() == true)
            {
                LoadModules();
                Application.Current.MainWindow.Show();
            }

            else
                Application.Current.Shutdown();
        }

        protected void LoadModules()
        {
            IModuleManager moduleManager = Container.Resolve<IModuleManager>();

            moduleManager.LoadModule(typeof(Admin.AdminModule).Name);
            moduleManager.LoadModule(typeof(Batches.BatchesModule).Name);
            moduleManager.LoadModule(typeof(Masters.MastersModule).Name);
            moduleManager.LoadModule(typeof(Methods.MethodsModule).Name);
            moduleManager.LoadModule(typeof(Projects.ProjectsModule).Name);
            moduleManager.LoadModule(typeof(Reports.ReportsModule).Name);
            moduleManager.LoadModule(typeof(Specifications.SpecificationsModule).Name);
            moduleManager.LoadModule(typeof(Standards.StandardsModule).Name);
            moduleManager.LoadModule(typeof(Tasks.TasksModule).Name);
        }
    }
}
