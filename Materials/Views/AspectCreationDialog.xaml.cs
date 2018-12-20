using LabDbContext;
using System.Windows;

namespace Materials.Views
{
    /// <summary>
    /// Logica di interazione per AspectCreationDialog.xaml
    /// </summary>
    public partial class AspectCreationDialog : Window
    {
        #region Constructors

        public AspectCreationDialog()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        public Aspect AspectInstance => (DataContext as ViewModels.AspectCreationDialogViewModel).AspectInstance;

        #endregion Properties
    }
}