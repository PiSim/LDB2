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

namespace Instruments.Views
{
    /// <summary>
    /// Logica di interazione per CalibrationReportEdit.xaml
    /// </summary>
    public partial class CalibrationReportEdit : UserControl, INavigationAware, IView
    {
        public CalibrationReportEdit()
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
            (DataContext as ViewModels.CalibrationReportEditViewModel).CalibrationInstance =
               ncontext.Parameters["ObjectInstance"] as CalibrationReport;
        }



        private void ReferenceTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && AddReferenceButton.Command.CanExecute(AddReferenceButton.CommandParameter))
                AddReferenceButton.Command.Execute(AddReferenceButton.CommandParameter);
        }
    }
}
