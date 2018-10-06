using Controls.Views;
using Microsoft.Practices.Prism.Mvvm;
using Prism.Regions;
using System.Windows.Controls;

namespace Tasks.Views
{
    /// <summary>
    /// Interaction logic for TaskEdit.xaml
    /// </summary>
    public partial class TaskEdit : UserControl, INavigationAware, IView
    {
        #region Constructors

        public TaskEdit(IRegionManager regionManager)
        {
            InitializeComponent();
            regionManager.RegisterViewWithRegion(RegionNames.TaskEditProjectDetailsRegion,
                                                typeof(ProjectDetailsControl));
        }

        #endregion Constructors

        #region Methods

        public bool IsNavigationTarget(NavigationContext ncontext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext ncontext)
        {
        }

        public void OnNavigatedTo(NavigationContext ncontext)
        {
            (DataContext as ViewModels.TaskEditViewModel).TaskInstance =
                 ncontext.Parameters["ObjectInstance"] as LabDbContext.Task;
        }

        #endregion Methods
    }
}