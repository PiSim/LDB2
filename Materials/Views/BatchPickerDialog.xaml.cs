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

namespace Materials.Views
{
    /// <summary>
    /// Interaction logic for BatchPickerDialog.xaml
    /// </summary>
    public partial class BatchPickerDialog : Window
    {
        private string _batchNumber;
        
        public BatchPickerDialog(DBEntities entities)
        {
            DataContext = new ViewModels.BatchPickerDialogViewModel(this);
            InitializeComponent();
        }
        
        public string BatchNumber
        {
            get { return _batchNumber; }
            set { _batchNumber = value; }
        }
    }
}
