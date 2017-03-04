using DBManager;
using Microsoft.Practices.Prism.Mvvm;
using Prism.Mvvm;
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

namespace Instruments.Views
{
    /// <summary>
    /// Interaction logic for InstrumentEdit.xaml
    /// </summary>
    public partial class InstrumentEdit : UserControl, IView, INavigationAware 
    {
        public InstrumentEdit()
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
            (DataContext as ViewModels.InstrumentEditViewModel).InstrumentInstance = 
               ncontext.Parameters["ObjectInstance"] as Instrument;
        }
    }
}
