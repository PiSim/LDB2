using DBManager;
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

namespace Reports.Views
{
    /// <summary>
    /// Interaction logic for ReportEditView.xaml
    /// </summary>
    public partial class ExternalReportEditView : UserControl, INavigationAware
    {
        private DBEntities _entities;
        private EventAggregator _eventAggregator;

        public ExternalReportEditView(DBEntities entities,
                                    EventAggregator aggregator)
        {
            _eventAggregator = aggregator;
            _entities = entities;
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
            ViewModels.ExternalReportEditViewModel viewModel =
                new ViewModels.ExternalReportEditViewModel(_entities,
                                                            _eventAggregator,
                                                            ncontext.Parameters["ObjectInstance"] as ExternalReport);
            DataContext = viewModel;
        }
    }
}