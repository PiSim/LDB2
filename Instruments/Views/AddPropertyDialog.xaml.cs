using LabDbContext;
using Microsoft.Practices.Prism.Mvvm;
using System.Windows;

namespace Instruments.Views
{
    /// <summary>
    /// Logica di interazione per AddPropertyDialog.xaml
    /// </summary>
    public partial class AddPropertyDialog : Window, IView
    {
        #region Constructors

        public AddPropertyDialog()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        public MeasurableQuantity QuantityInstance => (DataContext as ViewModels.AddPropertyDialogViewModel).SelectedQuantity;

        #endregion Properties
    }
}