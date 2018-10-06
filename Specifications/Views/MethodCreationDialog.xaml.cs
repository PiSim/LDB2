using LabDbContext;
using Microsoft.Practices.Prism.Mvvm;
using System.Windows;

namespace Specifications.Views
{
    /// <summary>
    /// Interaction logic for MethodCreationDialog.xaml
    /// </summary>
    public partial class MethodCreationDialog : Window, IView
    {
        #region Constructors

        public MethodCreationDialog()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        public Method MethodInstance => (DataContext as ViewModels.MethodCreationDialogViewModel).MethodInstance;

        #endregion Properties
    }
}