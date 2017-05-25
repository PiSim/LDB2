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

namespace Instruments.Views
{
    /// <summary>
    /// Interaction logic for InstrumentCreationDialog.xaml
    /// </summary>
    public partial class InstrumentCreationDialog : Window, IView
    {
        public InstrumentCreationDialog(DBEntities entities)
        {
            InitializeComponent();
        }

        public Instrument InstrumentInstance
        {
            get { return (DataContext as ViewModels.InstrumentCreationDialogViewModel).InstrumentInstance; }
        }
    }
}
