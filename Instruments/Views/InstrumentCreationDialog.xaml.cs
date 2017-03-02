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
    /// Interaction logic for InstrumentCreationDialog.xaml
    /// </summary>
    public partial class InstrumentCreationDialog : Window
    {
        private Instrument _instrumentInstance;

        public InstrumentCreationDialog(DBEntities entities)
        {
            DataContext = new ViewModels.InstrumentCreationDialogViewModel(this, entities);
            InitializeComponent();
        }

        public Instrument InstrumentInstance
        {
            get { return _instrumentInstance; }
            set { _instrumentInstance = value; }
        }
    }
}
