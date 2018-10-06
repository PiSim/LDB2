using LabDbContext;
using System.Windows;

namespace Specifications.Views
{
    /// <summary>
    /// Logica di interazione per ConsolidateStandardDialog.xaml
    /// </summary>
    public partial class ConsolidateStandardDialog : Window
    {
        #region Constructors

        public ConsolidateStandardDialog()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        public Std StandardInstance
        {
            get => (DataContext as ViewModels.ConsolidateStandardDialogViewModel).StandardInstance;
            set => (DataContext as ViewModels.ConsolidateStandardDialogViewModel).StandardInstance = value;
        }

        #endregion Properties
    }
}