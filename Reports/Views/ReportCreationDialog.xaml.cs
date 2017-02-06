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

namespace Reports.Views
{
    /// <summary>
    /// Interaction logic for ReportCreationDialog.xaml
    /// </summary>
    public partial class ReportCreationDialog : Window
    {
        private Report _reportInstance;
        
        public ReportCreationDialog(DBEntities entities)
        {
            DataContext = new ViewModels.ReportCreationViewModel(this, entities);
            InitializeComponent();
        }
        
        public Report ReportInstance
        {
            get { return _reportInstance; }
            set { _reportInstance = value; }
        }
    }
}
