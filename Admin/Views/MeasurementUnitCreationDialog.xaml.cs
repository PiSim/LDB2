using DBManager;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Admin.Views
{
    /// <summary>
    /// Logica di interazione per MeasurementUnitCreationDialog.xaml
    /// </summary>
    public partial class MeasurementUnitCreationDialog : Window, IView
    {
        public MeasurementUnitCreationDialog()
        {
            InitializeComponent();
        }

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
    }
}
