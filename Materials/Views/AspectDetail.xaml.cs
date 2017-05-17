using DBManager;
using Microsoft.Practices.Prism.Mvvm;
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

namespace Materials.Views
{
    /// <summary>
    /// Logica di interazione per AspectDetail.xaml
    /// </summary>
    public partial class AspectDetail : UserControl, IView, INavigationAware
    {
        public AspectDetail()
        {
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
            (DataContext as ViewModels.AspectDetailViewModel).AspectInstance =
               ncontext.Parameters["ObjectInstance"] as Aspect;
        }
    }
}
