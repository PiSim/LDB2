using Microsoft.Practices.Unity;
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

namespace Batches.Views
{

    public partial class SampleLogView : UserControl
    {
        public SampleLogView(DBManager.DBEntities entities, 
                            BatchServiceProvider batchServiceProvider,
                            IUnityContainer container)
        {
            DataContext = new ViewModels.SampleLogViewModel(entities, batchServiceProvider, container);
            InitializeComponent();
        }
    }
}
