using DBManager;
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

namespace Specifications.Views
{
    /// <summary>
    /// Logica di interazione per ConsolidateStandardDialog.xaml
    /// </summary>
    public partial class ConsolidateStandardDialog : Window
    {
        public ConsolidateStandardDialog()
        {
            InitializeComponent();
        }

        public Std StandardInstance
        {
            get => (DataContext as ViewModels.ConsolidateStandardDialogViewModel).StandardInstance;
            set => (DataContext as ViewModels.ConsolidateStandardDialogViewModel).StandardInstance = value;
        }
    }
}
