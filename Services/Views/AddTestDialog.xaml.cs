using DBManager;
using Infrastructure.Wrappers;
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

namespace Services.Views
{
    /// <summary>
    /// Interaction logic for AddTestDialog.xaml
    /// </summary>
    public partial class AddTestDialog : Window, IView
    {
        public AddTestDialog()
        {
            InitializeComponent();
        }

        public Report ReportInstance
        {
            get { return (DataContext as ViewModels.AddTestDialogViewModel).ReportInstance; }
            set
            {
                (DataContext as ViewModels.AddTestDialogViewModel).ReportInstance = value;
            }
        }

        public IEnumerable<ReportItemWrapper> TestList
        {
            get { return (DataContext as ViewModels.AddTestDialogViewModel).TestList; }
        }
    }
}
