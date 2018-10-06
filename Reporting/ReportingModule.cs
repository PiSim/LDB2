using Prism.Ioc;
using Prism.Modularity;

namespace Reporting
{
    [Module(ModuleName = "Reporting")]
    public class ReportingModule : IModule
    {
        #region Constructors

        public ReportingModule()
        {
        }

        #endregion Constructors

        #region Methods

        public void Initialize()
        {
        }

        public void OnInitialized(IContainerProvider containerProvider)
        {
        }

        public void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<ReportingEngine>();
            containerRegistry.Register<IReportingService, ReportingService>();
        }

        #endregion Methods
    }
}