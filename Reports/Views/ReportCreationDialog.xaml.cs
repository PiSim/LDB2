using DBManager;
using Infrastructure;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Reports.Views
{
    /// <summary>
    /// Interaction logic for ReportCreationDialog.xaml
    /// </summary>
    public partial class ReportCreationDialog : Window, IView
    {

        public ReportCreationDialog()
        {
            InitializeComponent();
        }

        public Batch Batch
        {
            set { (DataContext as ViewModels.ReportCreationDialogViewModel).BatchNumber = value.Number; }
        }

        public Report ReportInstance
        {
            get { return (DataContext as ViewModels.ReportCreationDialogViewModel).ReportInstance; }
        }
    }
}
