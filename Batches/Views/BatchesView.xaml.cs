using Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Events;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Batches.Views
{
    /// <summary>
    /// Interaction logic for BatchesView.xaml
    /// </summary>
    public partial class BatchesView : UserControl
    {
        IEventAggregator _eventAggregator;

        public BatchesView(IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            InitializeComponent();
        }

        private void OpenQueryView(object sender, RoutedEventArgs e)
        {
            _eventAggregator.GetEvent<NavigationRequested>().Publish(BatchesViewNames.BatchQueryView);
        }
    }
}
