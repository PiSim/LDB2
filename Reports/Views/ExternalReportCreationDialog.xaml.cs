using LabDbContext;
using Prism.Mvvm;
using System.Windows;

namespace Reports.Views
{
    /// <summary>
    /// Interaction logic for ExternalReportCreationDialog.xaml
    /// </summary>
    public partial class ExternalReportCreationDialog : Window
    {
        #region Constructors

        public ExternalReportCreationDialog()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        public ExternalReport ExternalReportInstance => (DataContext as ViewModels.ExternalReportCreationDialogViewModel).ExternalReportInstance;

        #endregion Properties
    }
}