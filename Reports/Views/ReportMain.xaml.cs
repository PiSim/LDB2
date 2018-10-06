using Microsoft.Practices.Prism.Mvvm;
using Prism.Regions;
using System.Windows.Controls;

namespace Reports.Views
{
    /// <summary>
    /// Interaction logic for ReportMainView.xaml
    /// </summary>
    public partial class ReportMain : UserControl, IView
    {
        #region Constructors

        public ReportMain(IRegionManager regionManager)
        {
            InitializeComponent();
        }

        #endregion Constructors
    }
}