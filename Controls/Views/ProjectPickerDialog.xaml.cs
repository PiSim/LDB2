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

namespace Controls.Views
{
    /// <summary>
    /// Interaction logic for ProjectPickerDialog.xaml
    /// </summary>
    public partial class ProjectPickerDialog : Window, IView
    {
        public ProjectPickerDialog()
        {
            InitializeComponent();
        }

        public Project ProjectInstance
        {
            get { return (DataContext as ViewModels.ProjectPickerDialogViewModel).SelectedProject; }
        }
    }
}
