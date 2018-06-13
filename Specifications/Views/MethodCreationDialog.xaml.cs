using DBManager;
using Microsoft.Practices.Prism.Mvvm;
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
using System.Windows.Shapes;

namespace Specifications.Views
{
    /// <summary>
    /// Interaction logic for MethodCreationDialog.xaml
    /// </summary>
    public partial class MethodCreationDialog : Window, IView
    {
        public MethodCreationDialog()
        {
            InitializeComponent();
        }

        public Method OldVersion
        {
            get => (DataContext as ViewModels.MethodCreationDialogViewModel).OldVersion;

            set => (DataContext as ViewModels.MethodCreationDialogViewModel).OldVersion = value;
        }

        public Method MethodInstance
        {
            get { return (DataContext as ViewModels.MethodCreationDialogViewModel).MethodInstance; }
        }
    }
}
