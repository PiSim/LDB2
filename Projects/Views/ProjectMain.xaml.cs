using Controls.Views;
using Microsoft.Practices.Prism.Mvvm;
using Prism.Regions;
using System.Windows.Controls;

namespace Projects.Views
{
    /// <summary>
    /// Interaction logic for ProjectMainView.xaml
    /// </summary>
    public partial class ProjectMain : UserControl, IView
    {
        #region Constructors

        public ProjectMain(IRegionManager regionManager)
        {
            InitializeComponent();
            regionManager.RegisterViewWithRegion(RegionNames.ProjectStatRegion,
                                                typeof(Views.ProjectStats));
        }

        #endregion Constructors
    }
}