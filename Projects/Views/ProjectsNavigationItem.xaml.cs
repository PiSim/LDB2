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

namespace Projects.Views
{
    /// <summary>
    /// Interaction logic for ProjectsNavigationItem.xaml
    /// </summary>
    public partial class ProjectsNavigationItem : UserControl, IModuleNavigationTag
    {
        public ProjectsNavigationItem()
        {
            InitializeComponent();
        }
        
        public string ViewName
        {
            get { return ProjectsViewNames.ProjectMainView; }
        }
    }
}
