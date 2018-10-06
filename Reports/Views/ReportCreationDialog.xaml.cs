using LabDbContext;
using Microsoft.Practices.Prism.Mvvm;
using System.Windows;

namespace Reports.Views
{
    /// <summary>
    /// Interaction logic for ReportCreationDialog.xaml
    /// </summary>
    public partial class ReportCreationDialog : Window, IView
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

        public ViewModels.ReportCreationDialogViewModel.CreationModes CreationMode
        {
            get => ViewModel.CreationMode;
            set => ViewModel.CreationMode = value;
        }

        public Task TaskInstance
        {
            get => ViewModel.TaskInstance;
            set => ViewModel.TaskInstance = value;
        }

        public ViewModels.ReportCreationDialogViewModel ViewModel => DataContext as ViewModels.ReportCreationDialogViewModel;

        #endregion Properties
    }
}