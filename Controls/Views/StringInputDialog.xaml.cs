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

namespace Controls.Views
{
    /// <summary>
    /// Interaction logic for OrganizationCreationDialog.xaml
    /// </summary>
    public partial class StringInputDialog : Window
    {
        public StringInputDialog()
        {
            DataContext = new ViewModels.StringInputDialogViewModel(this);
            InitializeComponent();
        }

        public string InputString
        {
            get { return (DataContext as ViewModels.StringInputDialogViewModel).InputString; }
        }

        public string Message
        {
            get { return (DataContext as ViewModels.StringInputDialogViewModel).Message; }
            set { (DataContext as ViewModels.StringInputDialogViewModel).Message = value; }
        }
    }
}
