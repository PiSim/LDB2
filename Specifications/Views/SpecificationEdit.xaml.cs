using DBManager;
using Infrastructure.Events;
using Microsoft.Practices.Prism.Mvvm;
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

namespace Specifications.Views
{
    /// <summary>
    /// Interaction logic for SpecificationEditView.xaml
    /// </summary>
    public partial class SpecificationEdit : UserControl, INavigationAware, IView
    {

        public SpecificationEdit()
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
            (DataContext as ViewModels.SpecificationEditViewModel).SpecificationInstance 
                = ncontext.Parameters["ObjectInstance"] as Specification;
        }
    }
}
