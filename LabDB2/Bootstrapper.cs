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
                new Prism.Modularity.ModuleInfo(ControlsModuleType.Name, 
                                                ControlsModuleType.AssemblyQualifiedName));

            Type SecurityModuleType = typeof(Security.SecurityModule);
            ModuleCatalog.AddModule(
                new Prism.Modularity.ModuleInfo(SecurityModuleType.Name,
                                                SecurityModuleType.AssemblyQualifiedName));

            Type AdminModuleType = typeof(Admin.AdminModule);
            ModuleCatalog.AddModule(
                new Prism.Modularity.ModuleInfo(AdminModuleType.Name, 
                                                AdminModuleType.AssemblyQualifiedName));

            Type BatchesModuleType = typeof(Batches.BatchesModule);
            ModuleCatalog.AddModule(
                new Prism.Modularity.ModuleInfo(BatchesModuleType.Name, 
                                                BatchesModuleType.AssemblyQualifiedName));

            Type MastersModuleType = typeof(Masters.MastersModule);
            ModuleCatalog.AddModule(
                new Prism.Modularity.ModuleInfo(MastersModuleType.Name, 
                                                MastersModuleType.AssemblyQualifiedName));

            Type MethodsModuleType = typeof(Methods.MethodsModule);
            ModuleCatalog.AddModule(
                new Prism.Modularity.ModuleInfo(MethodsModuleType.Name, 
                                                MethodsModuleType.AssemblyQualifiedName));

            Type ProjectsModuleType = typeof(Projects.ProjectsModule);
            ModuleCatalog.AddModule(
                new Prism.Modularity.ModuleInfo(ProjectsModuleType.Name, 
                                                ProjectsModuleType.AssemblyQualifiedName));

            Type ReportsModuleType = typeof(Reports.ReportsModule);
            ModuleCatalog.AddModule(
                new Prism.Modularity.ModuleInfo(ReportsModuleType.Name, 
                                                ReportsModuleType.AssemblyQualifiedName));

            Type SpecificationModuleType = typeof(Specifications.SpecificationsModule);
            ModuleCatalog.AddModule(
                new Prism.Modularity.ModuleInfo(SpecificationModuleType.Name, 
                                                SpecificationModuleType.AssemblyQualifiedName));

            Type StandardModuleType = typeof(Standards.StandardsModule);
            ModuleCatalog.AddModule(
                new Prism.Modularity.ModuleInfo(StandardModuleType.Name, 
                                                StandardModuleType.AssemblyQualifiedName));

            Type TaskModuleType = typeof(Tasks.TasksModule);
            ModuleCatalog.AddModule(
                new Prism.Modularity.ModuleInfo(TaskModuleType.Name, 
                                                TaskModuleType.AssemblyQualifiedName));
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
