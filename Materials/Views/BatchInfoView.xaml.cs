using DBManager;
using Microsoft.Practices.Unity;
using Prism.Events;
using Prism.Regions;
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

namespace Materials.Views
{
    /// <summary>
    /// Interaction logic for BatchInfoView.xaml
    /// </summary>
    public partial class BatchInfoView : UserControl, INavigationAware
    {
        private EventAggregator _aggregator;
        private UnityContainer _container;
        
        public BatchInfoView(EventAggregator aggregator,
                            UnityContainer container)
        {
            _aggregator = aggregator;
            _container = container;
            InitializeComponent();
        }

        public bool IsNavigationTarget(NavigationContext ncontext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext ncontext)
        {

        }

        public void OnNavigatedTo(NavigationContext ncontext)
        {
            ViewModels.BatchInfoViewModel viewModel =
                new ViewModels.BatchInfoViewModel(ncontext.Parameters["ObjectInstance"] as Batch,
                                                _aggregator,
                                                _container);
            DataContext = viewModel;
        }
    }
}
