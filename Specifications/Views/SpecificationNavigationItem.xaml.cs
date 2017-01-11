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

namespace Specifications.Views
{
    /// <summary>
    /// Interaction logic for SpecificationNavigationItem.xaml
    /// </summary>
    public partial class SpecificationNavigationItem : UserControl, Navigation.IModuleNavigationTag
    {
        public SpecificationNavigationItem()
        {
            InitializeComponent();
        }

        public string ViewName
        {
            get { return SpecificationsViewNames.SpecificationsMainView; }
        }
    }
}
