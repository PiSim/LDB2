using Microsoft.Practices.Prism.Mvvm;
using Prism.Regions;
using System.Windows.Controls;

namespace Specifications.Views
{
    /// <summary>
    /// Interaction logic for SpecificationMainView.xaml
    /// </summary>
    public partial class SpecificationMain : UserControl, IView
    {
        #region Constructors

        public SpecificationMain(IRegionManager regionManager)
        {
            InitializeComponent();
        }

        #endregion Constructors
    }
}