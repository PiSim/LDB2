using LabDbContext;
using Prism.Regions;
using System.Windows.Controls;

namespace Reports.Views
{
    /// <summary>
    /// Interaction logic for ReportEditView.xaml
    /// </summary>
    public partial class ExternalReportEdit : UserControl, INavigationAware
    {
        #region Constructors

        public ExternalReportEdit()
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
            (DataContext as ViewModels.ExternalReportEditViewModel).ExternalReportInstance =
                 ncontext.Parameters["ObjectInstance"] as ExternalReport;
        }

        #endregion Methods
    }
}