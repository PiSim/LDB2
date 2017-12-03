using DBManager;
using Infrastructure;
using Microsoft.Practices.Prism.Mvvm;
using Prism.Events;
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
    /// Interaction logic for NewCalibrationDialog.xaml
    /// </summary>
    public partial class NewCalibrationDialog : Window, IView
    {
        public NewCalibrationDialog()
        {
            InitializeComponent();
        }

        public Instrument InstrumentInstance
        {
            get { return (DataContext as ViewModels.NewCalibrationDialogViewModel).InstrumentInstance; }
            set { (DataContext as ViewModels.NewCalibrationDialogViewModel).InstrumentInstance = value; }
        }

        public CalibrationReport ReportInstance
        {
            get { return (DataContext as ViewModels.NewCalibrationDialogViewModel).ReportInstance; }
        }
        
    }
}
