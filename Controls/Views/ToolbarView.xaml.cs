using Infrastructure;
using Navigation;
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

namespace Controls.Views
{
    /// <summary>
    /// Interaction logic for ToolbarView.xaml
    /// </summary>
    public partial class ToolbarView : UserControl
    {
        private IEventAggregator _eventAggregator;
        private IRegionManager _regionManager;

        public ToolbarView(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            _eventAggregator = eventAggregator;
            _regionManager = regionManager;
            DataContext = new ViewModels.ToolbarViewModel(eventAggregator);
            InitializeComponent();
        }


        private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _eventAggregator.GetEvent<NavigationRequested>()
                .Publish("BatchesView");
        }
    }
}
