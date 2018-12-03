using LabDbContext;
using Prism.Mvvm;
using Prism.Regions;
using System.Windows.Controls;

namespace Specifications.Views
{
    /// <summary>
    /// Logica di interazione per ControlPlanEdit.xaml
    /// </summary>
    public partial class ControlPlanEdit : UserControl, INavigationAware
    {
        #region Constructors

        public ControlPlanEdit()
        {
            InitializeComponent();
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
            (DataContext as ViewModels.ControlPlanEditViewModel).ControlPlanInstance
                = ncontext.Parameters["ObjectInstance"] as ControlPlan;
        }

        #endregion Methods
    }
}