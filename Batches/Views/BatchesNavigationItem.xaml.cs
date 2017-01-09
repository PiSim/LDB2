using Microsoft.Practices.Unity;
using Navigation;
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

namespace Batches.Views
{
    /// <summary>
    /// Interaction logic for BatchesNavigationItem.xaml
    /// </summary>
    public partial class BatchesNavigationItem : UserControl, IModuleNavigationTag
    {
        private IUnityContainer _container;

        public BatchesNavigationItem(IUnityContainer container)
        {
            _container = container;
            InitializeComponent();
        }

        public string ViewName
        {
            get { return BatchesViewNames.BatchesView; }
        }
    }
}
