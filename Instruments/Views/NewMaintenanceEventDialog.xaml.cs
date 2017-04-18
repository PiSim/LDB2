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

namespace Instruments.Views
{
    /// <summary>
    /// Interaction logic for NewMaintenanceEventDialog.xaml
    /// </summary>
    public partial class NewMaintenanceEventDialog : Window
    {
        private InstrumentMaintenanceEvent _eventInstance;

        public NewMaintenanceEventDialog(DBEntities entities)
        {
            DataContext = new ViewModels.NewMaintenanceEventDialogViewModel(entities, this);
            InitializeComponent();
        }

        public InstrumentMaintenanceEvent EventInstance
        {
            get { return _eventInstance; }
            internal set { _eventInstance = value; } 
        }
    }
}
