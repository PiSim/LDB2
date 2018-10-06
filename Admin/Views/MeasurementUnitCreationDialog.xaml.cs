using LabDbContext;
using Microsoft.Practices.Prism.Mvvm;
using System.Windows;

namespace Admin.Views
{
    /// <summary>
    /// Logica di interazione per MeasurementUnitCreationDialog.xaml
    /// </summary>
    public partial class MeasurementUnitCreationDialog : Window, IView
    {
        #region Constructors

        public MeasurementUnitCreationDialog()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Properties

        public bool CanModifyQuantity
        {
            get { return (DataContext as ViewModels.MeasurementUnitCreationDialogViewModel).CanModifyQuantity; }
            set { (DataContext as ViewModels.MeasurementUnitCreationDialogViewModel).CanModifyQuantity = value; }
        }

        public MeasurableQuantity MeasurableQuantityInstance
        {
            get { return (DataContext as ViewModels.MeasurementUnitCreationDialogViewModel).SelectedMeasurableQuantity; }
            set
            {
                (DataContext as ViewModels.MeasurementUnitCreationDialogViewModel).SetQuantity(value);
            }
        }

        #endregion Properties
    }
}