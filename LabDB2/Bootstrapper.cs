using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Unity;
using Prism.Modularity;
using Prism.Unity;
using LabDB2.Views;
using Security;
using Security.Views;
using System;
using System.Threading;
using System.Windows;

namespace LabDB2
{
    class Bootstrapper : UnityBootstrapper
    {
        protected override void ConfigureModuleCatalog()
        {

            ViewModelLocationProvider.SetDefaultViewModelFactory((type) =>
            {
                return Container.Resolve(type);
            });

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
                new ModuleInfo()
                {
                    ModuleName = DBManagerModuleType.Name,
                    ModuleType = DBManagerModuleType.AssemblyQualifiedName,
                });

            Type ControlsModuleType = typeof(Controls.ControlsModule);
            ModuleCatalog.AddModule(
                new ModuleInfo()
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

            Type MaterialsModuleType = typeof(Materials.MaterialsModule);
            ModuleCatalog.AddModule(
                new ModuleInfo()
                {
                    ModuleName = MaterialsModuleType.Name,
                    ModuleType = MaterialsModuleType.AssemblyQualifiedName,
                    InitializationMode = InitializationMode.OnDemand
                });

            Type OrganizationsModuleType = typeof(Organizations.OrganizationsModule);
            ModuleCatalog.AddModule(
                new ModuleInfo()
                {
                    ModuleName = OrganizationsModuleType.Name,
                    ModuleType = OrganizationsModuleType.AssemblyQualifiedName,
                    InitializationMode = InitializationMode.OnDemand
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

            DBPrincipal _currentPrincipal = Thread.CurrentPrincipal as DBPrincipal;

            if (_currentPrincipal.IsInRole(RoleNames.Admin))
                moduleManager.LoadModule(typeof(Admin.AdminModule).Name);

            moduleManager.LoadModule(typeof(Materials.MaterialsModule).Name);
            moduleManager.LoadModule(typeof(Organizations.OrganizationsModule).Name);
            moduleManager.LoadModule(typeof(Projects.ProjectsModule).Name);
            moduleManager.LoadModule(typeof(Reports.ReportsModule).Name);
            moduleManager.LoadModule(typeof(Specifications.SpecificationsModule).Name);

            if (_currentPrincipal.IsInRole(RoleNames.LoadTasks))
                moduleManager.LoadModule(typeof(Tasks.TasksModule).Name);
        }
    }
}
