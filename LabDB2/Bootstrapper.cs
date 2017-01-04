using Microsoft.Practices.Unity;
using Prism.Unity;
using LabDB2.Views;
using System.Windows;

namespace LabDB2
{
    class Bootstrapper : UnityBootstrapper
    {
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
