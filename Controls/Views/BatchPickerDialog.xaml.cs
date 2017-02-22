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
    /// Interaction logic for BatchPickerDialog.xaml
    /// </summary>
    public partial class BatchPickerDialog : Window
    {
        private Batch _batchInstance;
        
        public BatchPickerDialog(DBEntities entities)
        {
            DataContext = new ViewModels.BatchPickerViewModel(entities);
            InitializeComponent();
        }
        
        public Batch BatchInstance
        {
            get { return _batchInstance; }
            set { _batchInstance = value; }
        }
    }
}
