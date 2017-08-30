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

namespace Instruments.Views
{
    /// <summary>
    /// Logica di interazione per AddPropertyDialog.xaml
    /// </summary>
    public partial class AddPropertyDialog : Window : IView
    {
        public AddPropertyDialog()
        {
            InitializeComponent();
        }

        public MeasurableQuantity QuantityInstance
        {
            get { return (DataContext as ViewModels.AddPropertyDialogViewModel).SelectedQuantity; }
        }
    }
}
