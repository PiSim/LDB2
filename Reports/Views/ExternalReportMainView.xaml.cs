using DBManager;
using Prism.Events;
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
    /// Interaction logic for ExternalReportMainView.xaml
    /// </summary>
    public partial class ExternalReportMainView : UserControl
    {
        public ExternalReportMainView(DBEntities entities, 
                                    EventAggregator aggregator)
        {
            DataContext = new ViewModels.ExternalReportMainViewModel(entities, aggregator);
            InitializeComponent();
        }
    }
}
