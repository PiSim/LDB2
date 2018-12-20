using LabDbContext;
using System.Windows;

namespace Reports.Views
{
    /// <summary>
    /// Interaction logic for ReportCreationDialog.xaml
    /// </summary>
    public partial class ReportCreationDialog : Window
    {
        #region Constructors

        public ReportCreationDialog()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        public Batch Batch
        {
            get => ViewModel.SelectedBatch;
            set { ViewModel.BatchNumber = value.Number; }
        }

        public ViewModels.ReportCreationDialogViewModel ViewModel => DataContext as ViewModels.ReportCreationDialogViewModel;

        #endregion Properties
    }
}