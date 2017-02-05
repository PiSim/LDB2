using DBManager;
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
    public partial class SpecificationEditView : UserControl, INavigationAware
    {
        private DBEntities _entities;

        public SpecificationEditView(DBEntities entities)
        {
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
            ViewModels.SpecificationEditViewModel viewModel =
                new ViewModels.SpecificationEditViewModel(_entities,
                                                        ncontext.Parameters["ObjectInstance"] as Specification);
            DataContext = viewModel;
        }
    }
}
