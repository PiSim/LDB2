using DBManager;
using Prism.Events;
using Prism.Regions;
using Prism.Unity;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    /// Interaction logic for BatchQueryView.xaml
    /// </summary>
    public partial class BatchQueryView : UserControl
    {
        public BatchQueryView(LabDBEntities entities, IRegionManager regionManager)
        {
            DataContext = new ViewModels.BatchQueryViewModel(entities, regionManager);
            InitializeComponent();
        }
    }
}
