using LabDbContext;
using Microsoft.Practices.Prism.Mvvm;
using System.Windows;

namespace Tasks.Views
{
    /// <summary>
    /// Interaction logic for TaskCreationDialog.xaml
    /// </summary>
    public partial class TaskCreationDialog : Window, IView
    {
        #region Constructors

        public TaskCreationDialog()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        public Task TaskInstance => (DataContext as ViewModels.TaskCreationDialogViewModel).TaskInstance;

        #endregion Properties
    }
}