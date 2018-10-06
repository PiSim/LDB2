using Microsoft.Practices.Prism.Mvvm;
using System.Windows;

namespace Controls.Views
{
    /// <summary>
    /// Interaction logic for MaterialCreationDialog.xaml
    /// </summary>
    public partial class MaterialCreationDialog : Window, IView
    {
        #region Constructors

        public MaterialCreationDialog()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        public string MaterialAspect => (DataContext as ViewModels.MaterialCreationDialogViewModel).Aspect;

        public string MaterialLine => (DataContext as ViewModels.MaterialCreationDialogViewModel).Line;

        public string MaterialRecipe => (DataContext as ViewModels.MaterialCreationDialogViewModel).Recipe;

        public string MaterialType => (DataContext as ViewModels.MaterialCreationDialogViewModel).Type;

        #endregion Properties
    }
}