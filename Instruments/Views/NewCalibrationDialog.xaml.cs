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
    /// Interaction logic for NewCalibrationDialog.xaml
    /// </summary>
    public partial class NewCalibrationDialog : Window
    {
        public NewCalibrationDialog(DBEntities entities)
        {
            DataContext = new ViewModels.NewCalibrationDialogViewModel(entities, this);
            InitializeComponent();
        }
    }
}
