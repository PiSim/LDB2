﻿using Infrastructure;
using LabDB2.Views;
using Microsoft.Practices.Prism.Mvvm;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Unity;
using System;
using System.Security.Principal;
using System.Threading;
using System.Windows;

namespace LabDB2
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        #region Methods

        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            ViewModelLocationProvider.SetDefaultViewModelFactory((type) =>
            {
                return Container.Resolve(type);
            });

            // Loading common modules

            Type DBManagerModuleType = typeof(LabDbContext.LabDbEntitiesModule);
            moduleCatalog.AddModule(
                new Prism.Modularity.ModuleInfo()
                {
                    ModuleName = DBManagerModuleType.Name,
                    ModuleType = DBManagerModuleType.AssemblyQualifiedName,
                    InitializationMode = Prism.Modularity.InitializationMode.OnDemand
                });

            Type InfrastructureModuleType = typeof(Infrastructure.InfrastructureModule);
            moduleCatalog.AddModule(
                new Prism.Modularity.ModuleInfo()
                {
                    ModuleName = InfrastructureModuleType.Name,
                    ModuleType = InfrastructureModuleType.AssemblyQualifiedName
                });

            Type ControlsModuleType = typeof(Controls.ControlsModule);
            moduleCatalog.AddModule(
                new ModuleInfo()
                {
                    ModuleName = ControlsModuleType.Name,
                    ModuleType = ControlsModuleType.AssemblyQualifiedName
                });

            Type ReportingModuleType = typeof(Reporting.ReportingModule);
            moduleCatalog.AddModule(
                new ModuleInfo()
                {
                    ModuleName = ReportingModuleType.Name,
                    ModuleType = ReportingModuleType.AssemblyQualifiedName
                });

            // Initializing modules to be loaded on demand

            Type AdminModuleType = typeof(Admin.AdminModule);
            moduleCatalog.AddModule(
                new Prism.Modularity.ModuleInfo()
                {
                    ModuleName = AdminModuleType.Name,
                    ModuleType = AdminModuleType.AssemblyQualifiedName,
                    InitializationMode = Prism.Modularity.InitializationMode.OnDemand
                });

            Type InstrumentModuleType = typeof(Instruments.InstrumentsModule);
            moduleCatalog.AddModule(
                new ModuleInfo()
                {
                    ModuleName = InstrumentModuleType.Name,
                    ModuleType = InstrumentModuleType.AssemblyQualifiedName,
                    InitializationMode = InitializationMode.OnDemand
                });

            Type MaterialsModuleType = typeof(Materials.MaterialsModule);
            moduleCatalog.AddModule(
                new ModuleInfo()
                {
                    ModuleName = MaterialsModuleType.Name,
                    ModuleType = MaterialsModuleType.AssemblyQualifiedName,
                    InitializationMode = InitializationMode.OnDemand
                });

            Type ProjectsModuleType = typeof(Projects.ProjectsModule);
            moduleCatalog.AddModule(
                new Prism.Modularity.ModuleInfo()
                {
                    ModuleName = ProjectsModuleType.Name,
                    ModuleType = ProjectsModuleType.AssemblyQualifiedName,
                    InitializationMode = Prism.Modularity.InitializationMode.OnDemand
                });

            Type ReportsModuleType = typeof(Reports.ReportsModule);
            moduleCatalog.AddModule(
                new Prism.Modularity.ModuleInfo()
                {
                    ModuleName = ReportsModuleType.Name,
                    ModuleType = ReportsModuleType.AssemblyQualifiedName,
                    InitializationMode = Prism.Modularity.InitializationMode.OnDemand
                });

            Type SpecificationModuleType = typeof(Specifications.SpecificationsModule);
            moduleCatalog.AddModule(
                new Prism.Modularity.ModuleInfo()
                {
                    ModuleName = SpecificationModuleType.Name,
                    ModuleType = SpecificationModuleType.AssemblyQualifiedName,
                    InitializationMode = Prism.Modularity.InitializationMode.OnDemand
                });

            Type TaskModuleType = typeof(Tasks.TasksModule);
            moduleCatalog.AddModule(
                new Prism.Modularity.ModuleInfo()
                {
                    ModuleName = TaskModuleType.Name,
                    ModuleType = TaskModuleType.AssemblyQualifiedName,
                    InitializationMode = Prism.Modularity.InitializationMode.OnDemand
                });

            Type UserModuleType = typeof(User.UserModule);
            moduleCatalog.AddModule(
                new Prism.Modularity.ModuleInfo()
                {
                    ModuleName = UserModuleType.Name,
                    ModuleType = UserModuleType.AssemblyQualifiedName,
                    InitializationMode = Prism.Modularity.InitializationMode.OnDemand
                });
        }

        protected override Window CreateShell()
        {
            return Container.Resolve<Shell>();
        }

        protected override void InitializeModules()
        {
            IModuleManager moduleManager = Container.Resolve<IModuleManager>();

            IPrincipal _currentPrincipal = Thread.CurrentPrincipal;

            moduleManager.LoadModule(typeof(LabDbContext.LabDbEntitiesModule).Name);

            if (_currentPrincipal.IsInRole(UserRoleNames.Admin))
                moduleManager.LoadModule(typeof(Admin.AdminModule).Name);

            moduleManager.LoadModule(typeof(Instruments.InstrumentsModule).Name);
            moduleManager.LoadModule(typeof(Materials.MaterialsModule).Name);
            moduleManager.LoadModule(typeof(Projects.ProjectsModule).Name);
            moduleManager.LoadModule(typeof(Reports.ReportsModule).Name);
            moduleManager.LoadModule(typeof(Reporting.ReportingModule).Name);
            moduleManager.LoadModule(typeof(Specifications.SpecificationsModule).Name);
            moduleManager.LoadModule(typeof(User.UserModule).Name);

            if (_currentPrincipal.IsInRole(UserRoleNames.TaskLoad))
                moduleManager.LoadModule(typeof(Tasks.TasksModule).Name);
        }

        protected override void InitializeShell(Window shell)
        {
            LoginDialog logger = Container.Resolve<LoginDialog>();

            LabDbContext.LabDbEntities entities = new LabDbContext.LabDbEntities();
            if (!entities.Database.Exists())
            {
                Application.Current.Shutdown();
            }

            if (logger.ShowDialog() == true)
            {
                AppDomain.CurrentDomain.SetThreadPrincipal(logger.AuthenticatedPrincipal);
                Application.Current.MainWindow = shell;
                LabDB2.Properties.Settings.Default.LastLogUser = logger.UserName;
                LabDB2.Properties.Settings.Default.Save();
                Current.MainWindow.Show();
            }
            else
                Application.Current.Shutdown();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.Register<IAuthenticationService, AuthenticationService>();

            containerRegistry.RegisterSingleton<NavigationService>();
            Container.Resolve<NavigationService>();
        }

        #endregion Methods
    }
}