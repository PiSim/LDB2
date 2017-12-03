using DBManager;
using DBManager.Services;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Controls.Views
{
    /// <summary>
    /// Interaction logic for BatchSelector.xaml
    /// </summary>
    
    public partial class BatchSelector : UserControl
    {
        private string _batchNumber;
        private IDataService _dataService;

        public BatchSelector(IDataService dataService)
        {
            _dataService = dataService;
            DataContext = this;
            InitializeComponent();
        }

        public string BatchNumber
        {
            get { return _batchNumber; }
            set
            {
                _batchNumber = value;
                SelectedBatch = _dataService.GetBatch(_batchNumber);
            }
        }

        public static readonly DependencyProperty BatchNumberProperty =
            DependencyProperty.Register("BatchNumber", typeof(string),
                typeof(string), new PropertyMetadata(""));

        public Batch SelectedBatch
        {
            get { return (Batch)GetValue(SelectedBatchProperty); }
            set { SetValue(SelectedBatchProperty, value); }
        }

        public static readonly DependencyProperty SelectedBatchProperty =
            DependencyProperty.Register("SelectedBatch", typeof(Batch),
                typeof(BatchSelector), new PropertyMetadata(null));
    }
}
