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
    public partial class BatchMainView : UserControl
    {
        IEventAggregator _eventAggregator;

        public BatchMainView(IEventAggregator eventAggregator)
        {
            DataContext = new ViewModels.BatchMainViewModel(eventAggregator);
            InitializeComponent();
        }

        private void OpenQueryView(object sender, RoutedEventArgs e)
        {
            _eventAggregator.GetEvent<NavigationRequested>().Publish(ViewNames.BatchQueryView);
        }
    }
}
