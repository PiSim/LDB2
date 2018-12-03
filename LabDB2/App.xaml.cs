using Infrastructure;
using LabDB2.Views;
using LabDbContext;
using Prism.Mvvm;
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
                    ModuleType = DBManagerModuleType.AssemblyQualifiedName
                });

            Type LInstModuleType = typeof(LInst.LInstModule);
            moduleCatalog.AddModule(
                new ModuleInfo()
                {
                    ModuleName = LInstModuleType.Name,
                    ModuleType = LInstModuleType.AssemblyQualifiedName
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

            moduleManager.LoadModule(typeof(LInst.LInstModule).Name);
            TryLogin();
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
        }

        protected override void InitializeShell(Window shell)
        {
            Current.MainWindow = shell;
        }

        private void TryLogin()
        {
            LInst.LInstContext entities = new LInst.LInstContext();
            LoginDialog logger = Container.Resolve<LoginDialog>();
            
            if (logger.ShowDialog() == true)
            {
                AppDomain.CurrentDomain.SetThreadPrincipal(logger.AuthenticatedPrincipal);
                LabDB2.Properties.Settings.Default.LastLogUser = logger.UserName;
                LabDB2.Properties.Settings.Default.Save();
                OpenShell();
            }
            else
                Application.Current.Shutdown();
        }

        private void OpenShell()
        {
            Current.MainWindow.Show();
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