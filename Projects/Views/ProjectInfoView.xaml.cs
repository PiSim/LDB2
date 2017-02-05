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

namespace Projects.Views
{
    
    public partial class ProjectInfoView : UserControl, INavigationAware
    {
        private DBEntities _entities;
        private EventAggregator _aggregator;

        public ProjectInfoView(DBEntities entities,
                                EventAggregator aggregator)
        {
            _aggregator = aggregator;
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
            ViewModels.ProjectInfoViewModel viewModel =
                new ViewModels.ProjectInfoViewModel(_entities,
                                                    _aggregator,
                                                    ncontext.Parameters["ObjectInstance"] as Project);
            DataContext = viewModel;
        }
    }
}
