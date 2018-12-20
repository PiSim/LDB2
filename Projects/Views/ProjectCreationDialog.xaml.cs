using LabDbContext;
using System.Windows;

namespace Projects.Views
{
    /// <summary>
    /// Interaction logic for ProjectCreationDialog.xaml
    /// </summary>
    public partial class ProjectCreationDialog : Window
    {
        #region Constructors

        public ProjectCreationDialog()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        public Project ProjectInstance => (DataContext as ViewModels.ProjectCreationDialogViewModel).ProjectInstance;

        #endregion Properties
    }
}