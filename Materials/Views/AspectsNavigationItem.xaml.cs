using Infrastructure;
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
    /// Interaction logic for AspectsNavigationItem.xaml
    /// </summary>
    public partial class AspectsNavigationItem : UserControl, IModuleNavigationTag
    {
        public AspectsNavigationItem()
        {
            InitializeComponent();
        }

        public string ViewName
        {
            get { return MaterialViewNames.AspectView; }
        }
    }
}
