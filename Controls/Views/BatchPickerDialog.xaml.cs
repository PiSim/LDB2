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

namespace Controls.Views
{
    /// <summary>
    /// Interaction logic for BatchPickerDialog.xaml
    /// </summary>
    public partial class BatchPickerDialog : Window, IView
    {
        public BatchPickerDialog()
        {
            InitializeComponent();
        }
        
        public string BatchNumber
        {
            get { return (DataContext as ViewModels.BatchPickerDialogViewModel).Number; }
            set { (DataContext as ViewModels.BatchPickerDialogViewModel).Number = value; }
        }
    }
}
