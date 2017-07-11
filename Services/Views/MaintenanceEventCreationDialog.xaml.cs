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

namespace Services.Views
{
    /// <summary>
    /// Logica di interazione per MaintenanceEventCreationDialog.xaml
    /// </summary>
    public partial class MaintenanceEventCreationDialog : Window
    {
        public MaintenanceEventCreationDialog()
        {
            InitializeComponent();
        }

        public InstrumentMaintenanceEvent InstrumentEventInstance
        {
            get { return (DataContext as ViewModels.MaintenanceEventCreationDialogViewModel).EventInstance; }
        }

        public Instrument InstrumentInstance
        {
            get { return (DataContext as ViewModels.MaintenanceEventCreationDialogViewModel).InstrumentInstance; }
            set { (DataContext as ViewModels.MaintenanceEventCreationDialogViewModel).InstrumentInstance = value; }
        }
    }
}
