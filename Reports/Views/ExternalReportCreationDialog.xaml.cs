using DBManager;
using Infrastructure;
using Microsoft.Practices.Prism.Mvvm;
using Microsoft.Practices.Unity;
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

namespace Reports.Views
{
    /// <summary>
    /// Interaction logic for ExternalReportCreationDialog.xaml
    /// </summary>
    public partial class ExternalReportCreationDialog : Window, IView
    {
        public ExternalReportCreationDialog()
        {
            InitializeComponent();
        }
        
        public ExternalReport ExternalReportInstance
        {
            get { return (DataContext as ViewModels.ExternalReportCreationDialogViewModel).ExternalReportInstance; }
        }
    }
}
