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

namespace Tasks.Views
{
    /// <summary>
    /// Interaction logic for ConversionReviewDialog.xaml
    /// </summary>
    public partial class ConversionReviewDialog : Window, IView
    {
        public ConversionReviewDialog()
        {
            InitializeComponent();
        }

        public Report ReportInstance
        {
            get { return (DataContext as ConversionReviewDialogViewModel).ReportInstance; }
        }

        public DBManager.Task TaskInstance
        {
            get { return (DataContext as ConversionReviewDialogViewModel).TaskInstance; }
            set
            {
                (DataContext as ConversionReviewDialogViewModel).TaskInstance = value;
            }
        }
    }
}
