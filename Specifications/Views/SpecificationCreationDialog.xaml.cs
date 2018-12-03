using LabDbContext;
using Prism.Mvvm;
using System.Windows;

namespace Specifications.Views
{
    /// <summary>
    /// Interaction logic for SpecificationCreationDialog.xaml
    /// </summary>
    public partial class SpecificationCreationDialog : Window
    {
        #region Constructors

        public SpecificationCreationDialog()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        public Specification SpecificationInstance => (DataContext as ViewModels.SpecificationCreationDialogViewModel).SpecificationInstance;

        #endregion Properties
    }
}