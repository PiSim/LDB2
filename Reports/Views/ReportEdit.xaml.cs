using LabDbContext;
using Prism.Mvvm;
using Prism.Regions;
using System.Windows.Controls;

namespace Reports.Views
{
    /// <summary>
    /// Interaction logic for ReportEditView.xaml
    /// </summary>
    public partial class ReportEdit : UserControl, INavigationAware
    {
        #region Constructors

        public ReportEdit()
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
            (DataContext as ViewModels.ReportEditViewModel).Instance = ncontext.Parameters["ObjectInstance"] as Report;
        }

        #endregion Methods
    }
}